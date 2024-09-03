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
    public ICommand ResetFilterCommand { get; private set; }

    public ObservableCollection<CardFilterModel> CardFilters { get; set; } = new ObservableCollection<CardFilterModel>()
        { new CardFilterModel() };
    public FilterViewModel(SuperManagerSelectionService superManagerSelectionService)
    {
        _superManagerSelectionService = superManagerSelectionService;
        ApplyFilterCommand = new RelayCommand(ApplyFilters);
        ResetFilterCommand = new RelayCommand(ResetFilters);
    }

    private void ResetFilters()
    {
        var masterPredicate = PredicateBuilder.New<SuperManager>(true);
        _superManagerSelectionService.ApplyFilters(masterPredicate);
    }

    private void ApplyFilters()
    {
        var masterPredicate = PredicateBuilder.New<SuperManager>(true); // Start with a true predicate

        foreach (CardFilterModel filter in CardFilters)
        {
            var filterExpression = filter.GetExpression();
            masterPredicate = masterPredicate.And(filterExpression);
        }

        // SuperManagerElement testi = new SuperManagerElement
        // {
        //     SuperManagerId = 0,
        //     SuperManager = null,
        //     ElementId = 1,
        //     Element = new Element("Nature"),
        //     RankRequirement = 0,
        //     EffectivenessType = "SE"
        // };
        // CardFilterModel cardFilterModel = new CardFilterModel
        // {
        //     Level = (50, null),
        //     RankRange = (null, null),
        //     Rank = null,
        //     Area = null,
        //     Rarity = null,
        //     Elements = new List<SuperManagerElement>(){ testi },
        //     Promoted = false,
        //     HasPassiveMultiplier = null,
        //     PassiveMultiplier = null
        // };
        

        //_superManagerSelectionService.ApplyFilters(cardFilterModel.GetExpression());
        _superManagerSelectionService.ApplyFilters(masterPredicate);
    }
}