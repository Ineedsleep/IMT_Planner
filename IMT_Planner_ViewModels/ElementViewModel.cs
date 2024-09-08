using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels;

public class ElementViewModel : ObservableObject
{
    private readonly Element? _element = null;

    public ElementViewModel()
    {
        
    }
    public ElementViewModel(Element element)
    {
        _element = element;
    }

    public string ElementName
    {
        get
        {
            if (_element?.Name != null) return _element?.Name;
            else return "";
        }
    }

    public ImageSource ImagePath
    {
        get
        {
            var imageName = _element.Name;
            if (imageName == null) return null;

            // Define the URI for the resource image
            var resourceUri = new Uri($"pack://application:,,,/Resources/Elements/{_element.Name}.png");

            // Try to load the image from the URI
            try
            {
                var bitmapImage = new BitmapImage(resourceUri);
                return bitmapImage;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load image: " + ex);
                return null;
            }
        }
    }
}