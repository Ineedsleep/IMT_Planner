using System.Collections.ObjectModel;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels;

public class SMElementalListViewModel : ObservableObject
{
    private readonly SuperManagerSelectionService _superManagerSelectionService;
    private ObservableCollection<SuperManagerElementViewModel> _elementCollection;

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
        InitElementalList();
        OnPropertyChanged(nameof(ElementCollection));
        var tmp = new ObservableCollection<SuperManagerElementViewModel>();
        OnPropertyChanged(nameof(SEElements));
    }

    private void InitElementalList()
    {
        ElementCollection = new ObservableCollection<SuperManagerElementViewModel>();
        var currentSm = _superManagerSelectionService.CurrentSuperManager;

        List<(string ElementName, string ElementType)> elementData = new List<(string, string)>
        {
            ("Nature", "SE"),
            ("Frost", "SE"),
            ("Flame", "SE"),
            ("Light", "SE"),
            ("Dark", "PE"),
            ("Wind", "PE"),
            ("Sand", "NVE"),
            ("Water", "NVE"),
        };

        foreach (var data in elementData)
        {
            SuperManagerElementViewModel elementViewModel = 
                new SuperManagerElementViewModel(new SuperManagerElement
                {
                   SuperManager = currentSm,
                    Element = new Element(data.ElementName),
                   EffectivenessType = data.ElementType    
                });
            ElementCollection.Add(elementViewModel);
        }
    }
}