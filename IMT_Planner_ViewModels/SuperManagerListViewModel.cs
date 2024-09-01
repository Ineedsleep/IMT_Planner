using System.Collections.ObjectModel;
using System.Windows.Input;
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
    public ICommand SaveCommand { get; private set; }
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
        SaveCommand = new RelayCommand(SaveSuperManager);
        SelectSuperManagerCommand = new RelayCommand<SuperManagerCardViewModel>(SelectSuperManagerViewModel);
        try
        {
            _superManagerSelectionService.CreateSuperManagerCollection(_repositoryService.GetAllSuperManagersWithElements());
            _superManagerSelectionService.CurrentSuperManager = _superManagerCollection.First().SuperManager;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void ImportSuperManagers()
    {
        var tmp = SuperManagerCollection.Select(s => s.SuperManager);
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
    
    private void SaveSuperManager()
    {
//Todo add functionality to export to csv / change name as well
    }

    private void LoadSuperManagersAsync(string filePath)
    {    
        filePath = "C:\\Users\\Tower\\Downloads\\SM_Sheet.csv";
         _superManagerSelectionService.LoadSuperManagersFromFileAsync(filePath);
    }
}
