using System.Globalization;
using System.Windows.Data;

namespace IMT_Planner.Views.Converter;

public class CurrentToMaxValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Tuple<int, int> unlockedData)
        {
            return $"unlocked {unlockedData.Item1}/{unlockedData.Item2}";
        }

        return "unlocked 0/0"; // Default value if unexpected type
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}