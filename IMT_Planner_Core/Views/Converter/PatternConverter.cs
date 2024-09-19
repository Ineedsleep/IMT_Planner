using System;
using System.Globalization;
using System.Windows.Data;

namespace IMT_Planner.Converters
{
    public class PatternConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Ensure the parameter is not null and is a valid integer
            if (parameter != null && int.TryParse(parameter.ToString(), out int paramValue))
            {
                return value.Equals(paramValue);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value && parameter != null && int.TryParse(parameter.ToString(), out int paramValue))
            {
                return paramValue;
            }
            return Binding.DoNothing;
        }
    }
}