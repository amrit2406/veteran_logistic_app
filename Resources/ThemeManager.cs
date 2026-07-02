using System;
using System.Windows;

namespace veteran_logistic.Themes
{
    /// <summary>
    /// Manages the application's UI theme by loading and applying resource dictionaries.
    /// </summary>
    public static class ThemeManager
    {
        /// <summary>
        /// Represents the available themes in the application.
        /// </summary>
        public enum Theme
        {
            Light,
            Dark // Placeholder for future implementation
        }

        /// <summary>
        /// Applies a specified theme to the current application.
        /// </summary>
        /// <param name="theme">The theme to apply.</param>
        public static void ApplyTheme(Theme theme)
        {
            var themeUri = GetThemeUri(theme);

            // Clear existing dictionaries to prevent resource conflicts (simplest approach)
            Application.Current.Resources.MergedDictionaries.Clear();

            // Attempt to load the theme. If the pack URI fails (assembly name mismatch
            // or resource resolution issue), fall back to a relative URI.
            try
            {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = themeUri });
            }
            catch (Exception ex)
            {
                // Try fallback: relative path inside the assembly where this code lives.
                var fallback = new Uri("/" + GetAssemblyName() + ";component/Resources/LightTheme.xaml", UriKind.Relative);
                try
                {
                    Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = fallback });
                }
                catch
                {
                    // If both attempts fail, rethrow original exception to preserve stack/inner details.
                    throw new InvalidOperationException($"Failed to load theme resource. Tried '{themeUri}' and '{fallback}'.", ex);
                }
            }
        }

        private static Uri GetThemeUri(Theme theme) => theme switch
        {
            Theme.Light => new Uri($"pack://application:,,,{GetAssemblyName()};component/Resources/LightTheme.xaml", UriKind.Absolute),
            _ => throw new ArgumentException($"Theme '{theme}' is not supported.", nameof(theme)),
        };

        private static string GetAssemblyName()
        {
            // Use the entry assembly if available (typical for app), otherwise use this assembly.
            var asm = System.Reflection.Assembly.GetEntryAssembly() ?? typeof(ThemeManager).Assembly;
            return asm.GetName().Name ?? string.Empty;
        }
    }
}
