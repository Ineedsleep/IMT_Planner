using System.Windows.Input;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IMT_Planner_ViewModels;

public class SuperManagerPlannerViewModel : ObservableObject
{

    private readonly SuperManagerSelectionService _superManagerSelectionService;
    private readonly SuperManagerRepositoryService _repositoryService;
    private Tuple<int, int> _unlockedSmCount;
    public ICommand ExportToCSVCommand { get; private set; }
    public ICommand LoadCommand { get; private set; }
    public ICommand ImportCommand { get; private set; }

    public SuperManagerPlannerViewModel(SuperManagerSelectionService smSelectionService,
        SuperManagerRepositoryService repositoryService)
    {

        _superManagerSelectionService = smSelectionService;
        _repositoryService = repositoryService;
        LoadCommand = new RelayCommand<string>(async path => LoadSuperManagersAsync());
        ImportCommand = new RelayCommand<string>(async path => ImportSuperManagers());
        ExportToCSVCommand = new RelayCommand(ExportSuperManagerToCSV);
        _unlockedSmCount = new Tuple<int, int>(0,0);
         _superManagerSelectionService.SuperManagerChanged -= HandleSuperManagerSelectionChanged;
         _superManagerSelectionService.SuperManagerChanged += HandleSuperManagerSelectionChanged;
        _superManagerSelectionService.SuperManagerCardUpdated -= HandleSuperManagerSelectionChanged;
        _superManagerSelectionService.SuperManagerCardUpdated += HandleSuperManagerSelectionChanged;
        _superManagerSelectionService.FilterChanged += HandleSuperManagerSelectionChanged;
        _superManagerSelectionService.FilterChanged += HandleSuperManagerSelectionChanged;
    }

    private void HandleSuperManagerSelectionChanged()
    {
        OnPropertyChanged(nameof(TotalPromoCount));
        OnPropertyChanged(nameof(TotalSmCount));
        OnPropertyChanged(nameof(CurrentPromoCount));
        OnPropertyChanged(nameof(UnlockedSmCount));
    }


    private void HandleSuperManagerSelectionChanged(string name)
    {
        OnPropertyChanged(name);
        OnPropertyChanged(nameof(TotalPromoCount));
        OnPropertyChanged(nameof(TotalSmCount));
        OnPropertyChanged(nameof(CurrentPromoCount));
        OnPropertyChanged(nameof(UnlockedSmCount));
    }

    public int UnlockedSmCount
    {
        get => _superManagerSelectionService.SuperManagerCollection.Where(x => x.Unlocked).Count();
    }    
    public int TotalSmCount
    {
        get => _superManagerSelectionService.SuperManagerCollection.Count;
    }
    
        
    public int TotalPromoCount
    {
        get => _superManagerSelectionService.SuperManagerCollection.Count * 5;
    }
    public int CurrentPromoCount
    {
        get => _superManagerSelectionService.SuperManagerCollection.Sum(x => x.Promoted);
    }
    private void ImportSuperManagers()
    {
        IEnumerable<SuperManager> tmp = _superManagerSelectionService.SuperManagerCollection.Select(s => s.SuperManager);
        _repositoryService.ImportSuperManagers(tmp);
    }
    private void ExportSuperManagerToCSV()
    {

        var filePath = $@".\Resources\SM_Sheet_{DateTime.Now:yy-MM-dd}.csv";
        _superManagerSelectionService.ExportToCSV(filePath);
    }

    private void LoadSuperManagersAsync()
    {
       string filePath = @".\Resources\SM_Sheet_Default.csv";
        _superManagerSelectionService.LoadSuperManagersFromFileAsync(filePath);
    }
}