using System.Windows.Media;
using System.Windows.Media.Imaging;
using IMT_Planner_Model;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.VisualBasic.CompilerServices;

namespace IMT_Planner_ViewModels;

public class SuperManagerElementViewModel : ObservableObject
{
    private string _rankRequirement;
    public SuperManagerElement Element { get; private set; }

    public int Index { get; }

    public string RankRequirement
    {
        get => $"Rank: {Element.RankRequirement}";
        set
        {
            if (value == _rankRequirement) return;
            _rankRequirement = value;
            OnPropertyChanged();
        }
    }

    public SuperManagerElementViewModel()
    {
    } 
    public SuperManagerElementViewModel(SuperManagerElement model, int index)
    {
        Index = index;
        Element = model;
    } 

    // Exposing the ElementId property
    public int ElementId
    {
        get
        {
            return Element.ElementId;
        }
        private set
        {
            Element.ElementId = value;
        }
    }

    // Exposing the EffectivenessType property
    public string EffectivenessType
    {
        get
        {
            return Element.EffectivenessType;
        }
        private set
        {
            Element.EffectivenessType = value;
        }
    }
    public string Name
    {
        get
        {
            return Element.Element.Name;
        }
        private set
        {
            Element.Element.Name = value;
        }
    }
    // Exposing a string property for the image, if Element has one
    public ImageSource ImagePath
    {
        get
        {
            var imageName = Element.Element?.Name;
            if (imageName == null) return null;

            // Define the URI for the resource image
            var resourceUri = new Uri($"pack://application:,,,/Resources/Elements/{Element.Element.Name}.png");

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
    public void ChangeElement(SuperManagerElementViewModel value)
    {
        this.Element = value.Element;
    }
}
