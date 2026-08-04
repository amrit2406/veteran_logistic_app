using System.Globalization;
using System.Windows.Data;

namespace veteran_logistic.Converters;

/// <summary>
/// Converts boolean values to Yes/No strings.
/// </summary>
public class BooleanToYesNoConverter : IValueConverter
{
    /// <summary>
    /// Converts boolean to Yes/No.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Yes" : "No";
        }

        return "No";
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
