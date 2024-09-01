using System.Collections.ObjectModel;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels;

public class SMElementalListViewModel : ObservableObject
{
    private readonly SuperManagerSelectionService _superManagerSelectionService;
    private ObservableCollection<SuperManagerElementViewModel> _elementCollection;
    private SuperManagerElementViewModel _selectedElement;
    private int _index;

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

    public int SelectedIndex { get; set; }
    public SuperManagerElementViewModel SelectedElement
    {
        get
        {
            return _selectedElement;
        }
        set
        {
            if(_selectedElement != null)
            SEElements[_selectedElement.Index].ChangeElement(value);
                _selectedElement = value;
            OnPropertyChanged(nameof(SEElements));
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

    private void InitElementalList()
    {
        // var currentSm = _superManagerSelectionService.CurrentSuperManager;
        //
        // List<(string ElementName, string ElementType)> elementData = new List<(string, string)>
        // {
        //     ("Nature", "SE"),
        //     ("Frost", "SE"),
        //     ("Flame", "SE"),
        //     ("Light", "SE"),
        //     ("Dark", "PE"),
        //     ("Wind", "PE"),
        //     ("Sand", "NVE"),
        //     ("Water", "NVE"),
        // };
        //
        // foreach (var data in elementData)
        // {
        //     SuperManagerElementViewModel elementViewModel = 
        //         new SuperManagerElementViewModel(new SuperManagerElement
        //         {
        //            SuperManager = currentSm,
        //             Element = new Element(data.ElementName),
        //            EffectivenessType = data.ElementType,
        //         },ElementCollection.Count);
        //     ElementCollection.Add(elementViewModel);
        // }
    }
}