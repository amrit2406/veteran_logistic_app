using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Models;
using veteran_logistic.Configuration.Options;

namespace veteran_logistic.Authentication.Services;

/// <summary>
/// Persists Remember Me preferences to a local JSON file.
/// Stores only username and Remember Me flag — never credentials or session data.
/// </summary>
public sealed class RememberMeService : IRememberMeService
{
    private const string SettingsFileName = "remember-me.json";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private readonly ILogger<RememberMeService> _logger;
    private readonly string _settingsFilePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="RememberMeService"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="applicationOptions">Application options for resolving the settings directory.</param>
    public RememberMeService(ILogger<RememberMeService> logger, IOptions<ApplicationOptions> applicationOptions)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var appFolderName = SanitizeDirectoryName(applicationOptions?.Value?.Name ?? "VeteranLogistics");
        var settingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            appFolderName,
            "Settings");

        _settingsFilePath = Path.Combine(settingsDirectory, SettingsFileName);
        EnsureSettingsDirectoryExists();
    }

    /// <inheritdoc />
    public async Task<RememberMeSettings> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_settingsFilePath))
        {
            return CreateDefaultSettings();
        }

        try
        {
            await using var stream = File.OpenRead(_settingsFilePath);
            var settings = await JsonSerializer.DeserializeAsync<RememberMeSettings>(stream, JsonOptions, cancellationToken)
                .ConfigureAwait(false);

            if (settings is null || !settings.RememberMe)
            {
                return CreateDefaultSettings();
            }

            settings.Username ??= string.Empty;
            return settings;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to load Remember Me settings from {SettingsFilePath}. Using defaults.", _settingsFilePath);
            return CreateDefaultSettings();
        }
    }

    /// <inheritdoc />
    public async Task SaveAsync(RememberMeSettings settings, CancellationToken cancellationToken = default)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        if (!settings.RememberMe)
        {
            await ClearAsync(cancellationToken).ConfigureAwait(false);
            return;
        }

        EnsureSettingsDirectoryExists();

        var payload = new RememberMeSettings
        {
            Username = settings.Username ?? string.Empty,
            RememberMe = true
        };

        await using var stream = File.Create(_settingsFilePath);
        await JsonSerializer.SerializeAsync(stream, payload, JsonOptions, cancellationToken).ConfigureAwait(false);

        _logger.LogDebug("Remember Me settings saved for user {Username}.", payload.Username);
    }

    /// <inheritdoc />
    public Task ClearAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (File.Exists(_settingsFilePath))
            {
                File.Delete(_settingsFilePath);
                _logger.LogDebug("Remember Me settings cleared.");
            }
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            _logger.LogWarning(ex, "Failed to clear Remember Me settings at {SettingsFilePath}.", _settingsFilePath);
        }

        return Task.CompletedTask;
    }

    private void EnsureSettingsDirectoryExists()
    {
        var directory = Path.GetDirectoryName(_settingsFilePath);
        if (string.IsNullOrEmpty(directory))
        {
            return;
        }

        try
        {
            Directory.CreateDirectory(directory);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create Remember Me settings directory at {SettingsDirectory}.", directory);
        }
    }

    private static RememberMeSettings CreateDefaultSettings() => new()
    {
        Username = string.Empty,
        RememberMe = false
    };

    private static string SanitizeDirectoryName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Concat(name.Select(ch => invalidChars.Contains(ch) ? '_' : ch)).Trim();
        return string.IsNullOrWhiteSpace(sanitized) ? "VeteranLogistics" : sanitized;
    }
}
