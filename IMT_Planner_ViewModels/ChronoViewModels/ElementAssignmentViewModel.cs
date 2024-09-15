using System.Collections.ObjectModel;
using IMT_Planner_ViewModels.GeneralViewModels;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels.ChronoViewModels;

public class ElementAssignmentViewModel : ObservableObject
{
    private readonly ChronoSelectionService _chronoSelectionService;
    private readonly SuperManagerRepositoryService _superManagerRepositoryService;
    public ObservableCollection<Item> Items { get; set; }

    public ElementAssignmentViewModel(ChronoSelectionService chronoSelectionService,
        SuperManagerRepositoryService superManagerRepositoryService)
    {
        _chronoSelectionService = chronoSelectionService;
        _superManagerRepositoryService = superManagerRepositoryService;


        var elements = _superManagerRepositoryService.GetAllElements();
        ICollection<ElementViewModel> elementViewModels = new List<ElementViewModel>();
        foreach (var e in elements)
        {
            elementViewModels.Add(new ElementViewModel(e));
        }

        Items = new ObservableCollection<Item>()
        { 
            new Item { Name = "E", ElementCollection = elementViewModels },
   
            new Item { Name = "MS1", ElementCollection = elementViewModels },
            new Item { Name = "MS2", ElementCollection = elementViewModels },
            new Item { Name = "MS3", ElementCollection = elementViewModels },
            new Item { Name = "W", ElementCollection = elementViewModels },
            new Item { Name = "MS4", ElementCollection = elementViewModels },
            new Item { Name = "MS5", ElementCollection = elementViewModels },
            new Item { Name = "MS6", ElementCollection = elementViewModels },
        };

    }
}

public class Item
{
    public string Name { get; set; }
    public int SelectedIndex { get; set; }
    public object SelectedElement { get; set; }
    public IEnumerable<ElementViewModel> ElementCollection { get; set; } // Replace 'Element' with the actual type
}