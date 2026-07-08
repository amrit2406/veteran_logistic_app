using System;
using System.Globalization;
using System.Windows.Data;

namespace veteran_logistic.Converters;

/// <summary>
/// Converts boolean values to their inverse.
/// True converts to False, False converts to True.
/// </summary>
public sealed class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return !boolValue;
        }
        return false;
    }
}
