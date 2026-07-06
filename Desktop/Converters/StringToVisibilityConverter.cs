using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace veteran_logistic.Converters;

/// <summary>
/// Converts string values to Visibility.
/// Empty or null strings convert to Collapsed, non-empty strings convert to Visible.
/// </summary>
public sealed class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            return string.IsNullOrWhiteSpace(stringValue) ? Visibility.Collapsed : Visibility.Visible;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
