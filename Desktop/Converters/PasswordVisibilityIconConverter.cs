using System;
using System.Globalization;
using System.Windows.Data;

namespace veteran_logistic.Converters;

/// <summary>
/// Converts boolean password visibility to icon.
/// True (visible) returns crossed eye icon, False (hidden) returns eye icon.
/// </summary>
public sealed class PasswordVisibilityIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isVisible)
        {
            return isVisible ? "👁‍🗨️" : "👁";
        }
        return "👁";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
