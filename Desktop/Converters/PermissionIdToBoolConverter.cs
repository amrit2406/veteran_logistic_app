using System;
using System.Globalization;
using System.Windows.Data;

namespace veteran_logistic.Converters;

/// <summary>
/// Converts a permission ID to a boolean value based on whether the permission is granted.
/// This converter is used in conjunction with the ViewModel's IsPermissionGranted method.
/// </summary>
public sealed class PermissionIdToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // The binding will call DataContext.IsPermissionGranted(permissionId)
        // This converter is a placeholder - the actual logic is in the ViewModel
        // We need to return the value from the ViewModel method call
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is int permissionId)
        {
            return (permissionId, boolValue);
        }
        return Binding.DoNothing;
    }
}
