using System.Collections.ObjectModel;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Models;
using IMT_Planner_ViewModels.Services;

public class SuperManagerFilterViewModel
{
    private readonly SuperManagerSelectionService _selectionService;
    public CardFilterModel FilterModel { get; set; } = new CardFilterModel();

    private List<SuperManager> AllSuperManagers { get; set; } 

    public SuperManagerFilterViewModel(SuperManagerSelectionService selectionService)
    {
        _selectionService = selectionService;
    }

    
    public void ApplyFilters()
    {
        var filterExpression = FilterModel.GetExpression();
        _selectionService.ApplyFilters(filterExpression);
    //
    //     // Filter SuperManagers and transform to SuperManagerViewModels.
    //     var filteredSuperManagers = AllSuperManagers.AsQueryable()
    //                                                 .Where(filterExpression)
    //                                                 .Select(sm => new SuperManagerViewModel(sm))
    //                                                 .ToList();
    //
    //     FilteredSuperManagers.Clear();
    //     foreach(var smv in filteredSuperManagers)
    //     {
    //         FilteredSuperManagers.Add(smv);
    //     }  
     }  
}