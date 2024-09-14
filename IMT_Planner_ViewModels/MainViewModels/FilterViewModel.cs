using System.Collections.ObjectModel;
using System.Windows.Input;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.GeneralViewModels;
using IMT_Planner_ViewModels.Models;
using IMT_Planner_ViewModels.Services;
using LinqKit;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IMT_Planner_ViewModels.MainViewModels;

public class FilterViewModel : ObservableObject
{
    private readonly SuperManagerSelectionService _superManagerSelectionService;
    private readonly SuperManagerRepositoryService _repositoryService;
    private readonly IOrderedEnumerable<string> _allTags;
    private readonly List<ElementViewModel?> _elementList;
    public ICommand ApplyFilterCommand { get; private set; }
    public ICommand ResetFiltersCommand { get; private set; }

    public ObservableCollection<CardFilterModel> CardFilters { get; set; } = new ObservableCollection<CardFilterModel>()
        { new CardFilterModel() };

    public FilterViewModel(SuperManagerSelectionService superManagerSelectionService,
        SuperManagerRepositoryService repositoryService)
    {
        _superManagerSelectionService = superManagerSelectionService;
        _repositoryService = repositoryService;
        ApplyFilterCommand = new RelayCommand(ApplyFilters);
        ResetFiltersCommand = new RelayCommand(ResetFilters);
        var tmp = _repositoryService.GetAllElements().ToList();
       _allTags = _superManagerSelectionService.GetAllTags();


      _elementList = new List<ElementViewModel?>();
        _elementList.Add(null);
        foreach (var element in tmp)
        {
            _elementList.Add(new ElementViewModel(element));
        }

        InitializeFilterViewModel();
    }

    private void InitializeFilterViewModel()
    {
        foreach (var filter in CardFilters)
        {
            filter.ElementBase = _elementList;

            foreach (var tag in _allTags)
            {
                filter.Tags.Add(new CardFilterModel.FilterTag() { Active = false, Name = tag });
            }
        }
    }


    private void ResetFilters()
    {
        int count = CardFilters.Count;

        CardFilters.Clear();
        for (int i = 0; i < count; i++)
        {
            CardFilters.Add(new CardFilterModel());
        }
        OnPropertyChanged(nameof(CardFilters));
        _superManagerSelectionService.ApplyFilters(CardFilters.First().GetExpression());
        InitializeFilterViewModel();
    }

    private void ApplyFilters()
    {
        var masterPredicate = PredicateBuilder.New<SuperManager>(true); // Start with a true predicate

        foreach (CardFilterModel filter in CardFilters)
        {
            var filterExpression = filter.GetExpression();
            masterPredicate = masterPredicate.And(filterExpression);
        }

        _superManagerSelectionService.ApplyFilters(masterPredicate);
    }
}