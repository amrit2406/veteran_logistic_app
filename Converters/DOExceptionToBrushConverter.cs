using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using veteran_logistic.Reports.DOStatusReport.DTOs;

namespace veteran_logistic.Converters;

/// <summary>
/// Converts DO exception type to a background brush for row highlighting.
/// </summary>
public class DOExceptionToBrushConverter : IValueConverter
{
    /// <summary>
    /// Converts DO exception type to a background brush.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DOExceptionType exceptionType)
        {
            return exceptionType switch
            {
                DOExceptionType.None => Brushes.Transparent,
                _ => new SolidColorBrush(Color.FromRgb(254, 202, 202)) // Light red for any exception
            };
        }

        return Brushes.Transparent;
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
