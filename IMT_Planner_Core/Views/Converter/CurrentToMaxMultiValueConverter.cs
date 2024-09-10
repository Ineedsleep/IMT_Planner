using System.Globalization;
using System.Windows.Data;

namespace IMT_Planner.Views.Converter;

public class CurrentToMaxMultiValueConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 && values[0] is int unlockedCount && values[1] is int totalCount)
        {
            return $"{unlockedCount}/{totalCount}";
        }
        return "99/99";  // Default value if inputs are unexpected
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}