using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using veteran_logistic.Reports.DOStatusReport.DTOs;

namespace veteran_logistic.Converters;

/// <summary>
/// Converts DO status to a background brush for row highlighting.
/// </summary>
public class DOStatusToBrushConverter : IValueConverter
{
    /// <summary>
    /// Converts DO status to a background brush.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DOStatus status)
        {
            return status switch
            {
                DOStatus.Completed => new SolidColorBrush(Color.FromRgb(220, 252, 231)), // Light green
                DOStatus.InTransit => new SolidColorBrush(Color.FromRgb(219, 234, 254)), // Light blue
                DOStatus.PaymentPending => new SolidColorBrush(Color.FromRgb(254, 249, 195)), // Light yellow
                DOStatus.BillPending => new SolidColorBrush(Color.FromRgb(253, 186, 116)), // Light orange
                DOStatus.Unloaded => new SolidColorBrush(Color.FromRgb(243, 244, 246)), // Light gray
                _ => Brushes.Transparent
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
