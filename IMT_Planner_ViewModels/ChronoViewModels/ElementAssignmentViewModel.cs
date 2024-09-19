using System.Collections.ObjectModel;
using System.Windows.Input;
using IMT_Planner_ViewModels.GeneralViewModels;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IMT_Planner_ViewModels.ChronoViewModels;

public class ElementAssignmentViewModel : ObservableObject
{
    private readonly ChronoSelectionService _chronoSelectionService;
    private readonly SuperManagerRepositoryService _superManagerRepositoryService;
    public ObservableCollection<MineEntityViewModel> ElementBoxes { get; set; }
    public ObservableCollection<ElementViewModel> ElementCollection => _chronoSelectionService.ElementCollection;

    public int SelectedPattern
    {
        get => _chronoSelectionService.SelectedPattern;
        set
        {
                _chronoSelectionService.SelectedPattern = value;
                UpdateElementBoxes(value);
                OnPropertyChanged(nameof(SelectedPattern));
        }
    }

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

        ElementBoxes = new ObservableCollection<MineEntityViewModel>();
        UpdateElementBoxes(SelectedPattern);
        OnPropertyChanged(nameof(SelectedPattern));
    }

    private void UpdateElementBoxes(int pattern)
    {
        ElementBoxes.Clear();
        ElementBoxes.Add(_chronoSelectionService.Elevator);


        foreach (var entity in _chronoSelectionService.MineShaftCollection)
        {
            if (ElementBoxes.Count == 5)
                ElementBoxes.Add(_chronoSelectionService.Warehouse);
            if (ElementBoxes.Count == pattern + 2)
                break;
            ElementBoxes.Add(entity);
        }
    }
}