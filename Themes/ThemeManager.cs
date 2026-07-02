using System;
using System.Linq;
using System.Windows;

namespace veteran_logistic.Themes;

public static class ThemeManager
{
    public enum Theme
    {
        Light,
        Dark
    }

    public static Theme CurrentTheme { get; private set; } = Theme.Light;

    public static void ApplyTheme(Theme theme)
    {
        if (Application.Current is null)
        {
            throw new InvalidOperationException("The application must be initialized before applying a theme.");
        }

        var themeUri = GetThemeUri(theme);
        var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        var existingThemeDictionary = mergedDictionaries.FirstOrDefault(IsThemeDictionary);

        if (existingThemeDictionary is not null)
        {
            mergedDictionaries.Remove(existingThemeDictionary);
        }

        mergedDictionaries.Add(new ResourceDictionary { Source = themeUri });
        CurrentTheme = theme;
    }

    private static Uri GetThemeUri(Theme theme)
    {
        return theme switch
        {
            Theme.Light => new Uri("/Themes/LightTheme.xaml", UriKind.Relative),
            Theme.Dark => throw new NotImplementedException("Dark theme support is reserved for a future phase."),
            _ => throw new ArgumentOutOfRangeException(nameof(theme), theme, "Unknown theme."),
        };
    }

    private static bool IsThemeDictionary(ResourceDictionary dictionary)
    {
        var source = dictionary.Source?.OriginalString;
        return source is not null && source.Contains("/Themes/", StringComparison.OrdinalIgnoreCase);
    }
}
