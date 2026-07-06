using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;

namespace veteran_logistic.Administration.Users.Services;

/// <summary>
/// Implementation of the user command service.
/// </summary>
public sealed class UserCommandService : IUserCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICreateUserValidator _createValidator;
    private readonly IUpdateUserValidator _updateValidator;
    private readonly ILogger<UserCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="passwordHasher">The password hasher.</param>
    /// <param name="createValidator">The user creation validator.</param>
    /// <param name="updateValidator">The user update validator.</param>
    /// <param name="logger">The logger.</param>
    public UserCommandService(
        VeteranLogisticsDbContext dbContext,
        IPasswordHasher passwordHasher,
        ICreateUserValidator createValidator,
        IUpdateUserValidator updateValidator,
        ILogger<UserCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateUserResult> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateUserResult.Failure(errorMessage);
            }

            var existingUser = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken)
                .ConfigureAwait(false);

            if (existingUser is not null)
            {
                return CreateUserResult.Failure("A user with this username already exists.");
            }

            var passwordHashResult = _passwordHasher.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                DisplayName = request.DisplayName,
                PasswordHash = passwordHashResult.Hash,
                PasswordSalt = passwordHashResult.Salt,
                Role = request.Role,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return CreateUserResult.Success(user.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating user '{Username}'", request.Username);
            return CreateUserResult.Failure("An unexpected error occurred while creating the user.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateUserResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateUserResult.Failure(errorMessage);
            }

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
                .ConfigureAwait(false);

            if (user is null)
            {
                return UpdateUserResult.Failure("User not found.");
            }

            user.DisplayName = request.DisplayName;
            user.Role = request.Role;
            user.IsActive = request.IsActive;

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return UpdateUserResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating user '{UserId}'", request.UserId);
            return UpdateUserResult.Failure("An unexpected error occurred while updating the user.");
        }
    }
}
