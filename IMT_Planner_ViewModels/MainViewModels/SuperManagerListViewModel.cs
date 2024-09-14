using System.Collections.ObjectModel;
using System.Windows.Input;
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
            Console.WriteLine(e);
        }
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
