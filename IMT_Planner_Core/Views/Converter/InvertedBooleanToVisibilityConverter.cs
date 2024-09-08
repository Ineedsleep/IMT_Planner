using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace IMT_Planner.Views.Converter;

public class InvertedBooleanToVisibilityConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(value is bool)) return DependencyProperty.UnsetValue;
        return (bool)value ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(value is Visibility)) return DependencyProperty.UnsetValue;
        return (Visibility)value != Visibility.Visible;
    }
}