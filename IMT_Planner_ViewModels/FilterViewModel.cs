using System.Collections.ObjectModel;
using System.Windows.Input;
using IMT_Planner_ViewModels.Models;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IMT_Planner_ViewModels;

public class FilterViewModel : ObservableObject
{
    private readonly SuperManagerSelectionService _superManagerSelectionService;
    public ICommand ApplyFilterCommand { get; private set; }
    public ICommand ResetFilterCommand { get; private set; }
    public ObservableCollection<CardFilterModel> CardFilters { get; set; }
    public FilterViewModel(SuperManagerSelectionService superManagerSelectionService)
    {
        _superManagerSelectionService = superManagerSelectionService;
        ApplyFilterCommand = new RelayCommand(ApplyFilters);
        ResetFilterCommand = new RelayCommand(ResetFilters);
    }

    private void ResetFilters()
    {
        throw new NotImplementedException();
    }

    private void ApplyFilters()
    {
        throw new NotImplementedException();
    }
}