using System.Collections.ObjectModel;
using IMT_Planner_ViewModels.GeneralViewModels;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels.ChronoViewModels;

public class ElementAssignmentViewModel : ObservableObject
{
    private readonly ChronoSelectionService _chronoSelectionService;
    private readonly SuperManagerRepositoryService _superManagerRepositoryService;
    public ObservableCollection<MineEntityViewModel> Items { get; set; }
    public ObservableCollection<ElementViewModel> ElementCollection => _chronoSelectionService.ElementCollection;

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
            _chronoSelectionService.ElementCollection.Add(new ElementViewModel(e));
        }

        Items = new ObservableCollection<MineEntityViewModel>();
        foreach (var entity in _chronoSelectionService.MineShaftCollection)
        {
            if (Items.Count == 0)
                Items.Add(_chronoSelectionService.Elevator);
            if (Items.Count == 4)
                Items.Add(_chronoSelectionService.Warehouse);
            if (Items.Count == 8)
                break;
            Items.Add(entity);
        }
    }
}

public class Item
{
    public string Name { get; set; }
    public int SelectedIndex { get; set; }
    public object SelectedElement { get; set; }
}