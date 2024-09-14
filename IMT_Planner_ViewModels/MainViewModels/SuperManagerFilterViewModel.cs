using IMT_Planner_Model;
using IMT_Planner_ViewModels.Models;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels.MainViewModels;

public class SuperManagerFilterViewModel : ObservableObject
{
    private readonly SuperManagerSelectionService _selectionService;
    private readonly SuperManagerRepositoryService _repositoryService;
    private IEnumerable<Element> _elements;
    public CardFilterModel FilterModel { get; set; } = new CardFilterModel();

    private List<SuperManager> AllSuperManagers { get; set; } 

    public SuperManagerFilterViewModel(SuperManagerSelectionService selectionService,SuperManagerRepositoryService repositoryService)
    {
        _selectionService = selectionService;
        _repositoryService = repositoryService;
        Elements = _repositoryService.GetAllElements();
    }

    public IEnumerable<Element> Elements
    {
        get => _elements;
        set
        {
            _elements = value;
            OnPropertyChanged(nameof(Elements));
        }
    }


    public void ApplyFilters()
    {
        var filterExpression = FilterModel.GetExpression();
        _selectionService.ApplyFilters(filterExpression);
    }  
}