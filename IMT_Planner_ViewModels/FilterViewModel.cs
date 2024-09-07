using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Models;
using IMT_Planner_ViewModels.Services;
using LinqKit;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IMT_Planner_ViewModels;

public class FilterViewModel : ObservableObject
{
    private readonly SuperManagerSelectionService _superManagerSelectionService;
    public ICommand ApplyFilterCommand { get; private set; }
    public ICommand ResetFiltersCommand { get; private set; }

    public ObservableCollection<CardFilterModel> CardFilters { get; set; } = new ObservableCollection<CardFilterModel>()
        { new CardFilterModel() };
    public FilterViewModel(SuperManagerSelectionService superManagerSelectionService)
    {
        _superManagerSelectionService = superManagerSelectionService;
        ApplyFilterCommand = new RelayCommand(ApplyFilters);
        ResetFiltersCommand = new RelayCommand(ResetFilters);
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