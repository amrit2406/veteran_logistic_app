using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace veteran_logistic.Converters;

/// <summary>
/// Converts a dictionary key to its value for data binding.
/// </summary>
public class DictionaryValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Dictionary<string, object?> dictionary && parameter is string key)
        {
            var found = dictionary.TryGetValue(key, out var result);
            return found ? result ?? string.Empty : string.Empty;
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
