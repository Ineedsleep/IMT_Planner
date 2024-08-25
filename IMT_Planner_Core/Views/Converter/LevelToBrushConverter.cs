
using System.Globalization;
using System.Linq.Expressions;
using System.Windows.Data;
using System.Windows.Media;
using IMT_Planner_Model;

namespace IMT_Planner.Views.Converter;


public class MaxToBrushConverter: IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if(values.Count() == 0 || values.Any(v => v == null))
            return Binding.DoNothing;

        // Your conversion logic here using values[] array
        var level = (byte) values[0];
        var promo = (bool) values[1];
        
        // Based on val1 and val2 return required Brush 
        if (level == 50 && promo == true)
        {
            return Brushes.Gold;
        }
        return Brushes.Transparent;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class RankToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int level && level == 5)
        {
            return Brushes.Gold;
        }

        return Brushes.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class RarityToBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is Rarity rarity)
            switch (rarity)
            {
                case Rarity.Common:
                    return Brushes.LightGray;
                    case Rarity.Rare:
                    return Brushes.CornflowerBlue;
                    case Rarity.Epic:
                    return Brushes.BlueViolet;
                    case Rarity.Legendary:
                    return Brushes.PaleGoldenrod;
                    default:
                    return Brushes.Transparent;
                    
            }
        return Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}