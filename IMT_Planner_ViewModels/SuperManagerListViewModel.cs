using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using CsvHelper;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IMT_Planner_ViewModels;

public class SuperManagerListViewModel: ObservableObject
{
    private ObservableCollection<SuperManagerCardViewModel> _superManagerCollection;   
    private readonly SuperManagerSelectionService _superManagerSelectionService;
    private readonly SuperManagerRepositoryService _repositoryService;
    public ICommand ExportToCSVCommand { get; private set; }
    public ICommand LoadCommand { get; private set; }
    public ICommand ImportCommand { get; private set; }

    public ObservableCollection<SuperManagerCardViewModel> SuperManagerCollection
    {
        get { return _superManagerSelectionService.SuperManagerCollection; }
        set
        {
            _superManagerSelectionService.SuperManagerCollection = value;
            OnPropertyChanged();
        }
    }
    
    public SuperManagerListViewModel(SuperManagerSelectionService smSelectionService, SuperManagerRepositoryService repositoryService)
    {
        
        _superManagerSelectionService = smSelectionService;
        _repositoryService = repositoryService;
        _superManagerSelectionService.SuperManagerChanged -= HandleSuperManagerSelectionChanged;
        _superManagerSelectionService.SuperManagerChanged += HandleSuperManagerSelectionChanged;
        _superManagerCollection = new ObservableCollection<SuperManagerCardViewModel> ();
        LoadCommand = new RelayCommand<string>(async path => LoadSuperManagersAsync(path));
        ImportCommand = new RelayCommand<string>(async path => ImportSuperManagers());
        ExportToCSVCommand = new RelayCommand(ExportSuperManagerToCSV);
        SelectSuperManagerCommand = new RelayCommand<SuperManagerCardViewModel>(SelectSuperManagerViewModel);
        try
        {
            _superManagerSelectionService.CreateElementCollection(repositoryService.GetAllElements());
            _superManagerSelectionService.CreateSuperManagerCollection(_repositoryService.GetAllSuperManagersWithElements());
            _superManagerSelectionService.CurrentSuperManager = SuperManagerCollection.First().SuperManager;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void ImportSuperManagers()
    {
        IEnumerable<SuperManager> tmp = SuperManagerCollection.Select(s => s.SuperManager);
        _repositoryService.ImportSuperManagers(tmp);
    }

    public IRelayCommand SelectSuperManagerCommand { get; }
    

    private void SelectSuperManagerViewModel(SuperManagerCardViewModel superManagerViewModel)
    {
        _superManagerSelectionService.UpdateSelectedSuperManager(superManagerViewModel);
    }
    private void HandleSuperManagerSelectionChanged(string name)
    {
       OnPropertyChanged(name);
    }
    
    private void ExportSuperManagerToCSV()
    { 
        
        var filePath = $@".\Resources\SM_Sheet_{DateTime.Now:yy-MM-dd}.csv";
        _superManagerSelectionService.ExportToCSV(filePath);
    }

    private void LoadSuperManagersAsync(string filePath)
    {    
        filePath = @".\Resources\SM_Sheet_Default.csv";
         _superManagerSelectionService.LoadSuperManagersFromFileAsync(filePath);
    }
}
