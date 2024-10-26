using System.Collections.ObjectModel;
using System.Windows.Input;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IMT_Planner_ViewModels.MainViewModels;

public class SuperManagerListViewModel: ObservableObject
{
    private ObservableCollection<SuperManagerCardViewModel> _superManagerCollection;   
    private readonly SuperManagerSelectionService _superManagerSelectionService;
    private readonly SuperManagerRepositoryService _repositoryService;
    public ICommand ImportCommand { get; private set; }

    public ObservableCollection<SuperManagerCardViewModel> SuperManagerCollection
    {
        get => _superManagerSelectionService.SuperManagerCollection;
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
        _superManagerSelectionService.CollectionChanged -= HandleCollectionChanged;
        _superManagerSelectionService.CollectionChanged += HandleCollectionChanged;
        _superManagerCollection = new ObservableCollection<SuperManagerCardViewModel>();
        SelectSuperManagerCommand = new RelayCommand<SuperManagerCardViewModel>(SelectSuperManagerViewModel);
        try
        {
            _superManagerSelectionService.CreateElementCollection(repositoryService.GetAllElements());
            _superManagerSelectionService.CreatePassiveNameCollection(repositoryService.GetAllPassiveNames());
            _superManagerSelectionService.CreateSuperManagerCollection(_repositoryService.GetAllSuperManagersWithElements());
            _superManagerSelectionService.CurrentSuperManager = SuperManagerCollection.First().SuperManager;
       
        }
        catch (Exception e)
        {
            Console.WriteLine("No Super managers in DB yet, load from default csv file and save into DB");
            if(_superManagerSelectionService.SuperManagerCollection.Count == 0)
                _superManagerSelectionService.LoadSuperManagersFromFileAsync(@".\Resources\SM_Sheet_Default.csv");
            IEnumerable<SuperManager> tmp = _superManagerSelectionService.SuperManagerCollection.Select(s => s.SuperManager);
            _repositoryService.ImportSuperManagers(tmp);
        }
    }

    private void HandleCollectionChanged()
    {
        OnPropertyChanged(nameof(SuperManagerCollection));
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
}
