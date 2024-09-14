using System.Collections.ObjectModel;
using IMT_Planner_ViewModels.GeneralViewModels;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels.MainViewModels;

public class SMElementalListViewModel : ObservableObject
{
    private readonly SuperManagerSelectionService _superManagerSelectionService;
    private ObservableCollection<SuperManagerElementViewModel> _elementCollection;
    private SuperManagerElementViewModel _selectedElement;

    public ObservableCollection<SuperManagerElementViewModel> ElementCollection
    {
        get => _elementCollection;
        set => _elementCollection = value;
    }

    public ObservableCollection<SuperManagerElementViewModel> SEElements
    {
        get => _superManagerSelectionService.SEElements;
        private set
        {
            _superManagerSelectionService.SEElements = value;
            OnPropertyChanged();
        }
    }
    public SuperManagerElementViewModel SelectedElement
    {
        get
        {
            return _selectedElement;
        }
        set
        {
                _selectedElement = value;
        }
    }


    public ObservableCollection<SuperManagerElementViewModel> SECollection  
    {
        get => _superManagerSelectionService.SEElements;
    }  

    public ObservableCollection<SuperManagerElementViewModel> NVEElements
    {
        get => _superManagerSelectionService.NVEElements;
    }  
    public ObservableCollection<SuperManagerElementViewModel> PEElements
    {
        get => _superManagerSelectionService.PEElements;
    }

    public SMElementalListViewModel(SuperManagerSelectionService superManagerSelectionService)
    {
        _superManagerSelectionService = superManagerSelectionService;
        OnPropertyChanged(nameof(ElementCollection));
        var tmp = new ObservableCollection<SuperManagerElementViewModel>();
        OnPropertyChanged(nameof(SEElements));
        _superManagerSelectionService.ElementsChanged -= ElementUpdate;
        _superManagerSelectionService.ElementsChanged += ElementUpdate;
    }

    private void ElementUpdate()
    {
        OnPropertyChanged(nameof(SEElements));
        OnPropertyChanged(nameof(NVEElements));
        OnPropertyChanged(nameof(PEElements));
        ElementCollection = new ObservableCollection<SuperManagerElementViewModel>(SEElements.Concat(PEElements).Concat(NVEElements));
        
    }
    
}