using System.Windows.Media;
using System.Windows.Media.Imaging;
using IMT_Planner_Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels;

public class SuperManagerElementViewModel : ObservableObject
{
    private SuperManagerElement _model;

    public SuperManagerElementViewModel(SuperManagerElement model)
    {
        _model = model;
    } 

    // Exposing the ElementId property
    public int ElementId
    {
        get
        {
            return _model.ElementId;
        }
    }

    // Exposing the EffectivenessType property
    public string EffectivenessType
    {
        get
        {
            return _model.EffectivenessType;
        }
    }
    public string Name
    {
        get
        {
            return _model.Element.Name;
        }
    }
    // Exposing a string property for the image, if Element has one
    public ImageSource ImagePath
    {
        get
        {
            var imageName = _model.Element?.Name;
            if (imageName == null) return null;

            // Define the URI for the resource image
            var resourceUri = new Uri($"pack://application:,,,/Resources/Elements/{_model.Element.Name}.png");

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
    
    

    // Add other properties of Element and SuperManagerElement as needed

    // Add other properties of the SuperManagerElement as needed 
}
