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
    }
}