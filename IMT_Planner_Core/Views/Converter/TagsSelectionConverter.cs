using System.Globalization;
using System.Windows.Data;
using IMT_Planner_ViewModels.Models;

namespace IMT_Planner.Views.Converter;

public class TagsSelectionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var selectedTags = value as List<string>;
        var currentTag = parameter as string;

        return selectedTags != null && currentTag != null && selectedTags.Contains(currentTag);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isSelected = (bool)value;
        var currentTag = parameter as string;
        var viewModelType = targetType.GetGenericTypeDefinition().GetGenericArguments()[0];
        var propertyInfo = viewModelType.GetProperty("SelectedTags");

        if (propertyInfo != null)
        {
            var filterModelInstance = Activator.CreateInstance(viewModelType);
            var selectedTags = propertyInfo.GetValue(filterModelInstance) as List<string>;

            if (isSelected && !selectedTags.Contains(currentTag))
            {
                selectedTags.Add(currentTag);
            }
            else if (!isSelected && selectedTags.Contains(currentTag))
            {
                selectedTags.Remove(currentTag);
            }

            return selectedTags;
        }

        return null;
    }
}