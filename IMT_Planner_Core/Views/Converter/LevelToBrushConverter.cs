
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace IMT_Planner.Views.Converter;
public class LevelToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is byte level && level == 50)
        {
            return Brushes.Green;
        }

        return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}