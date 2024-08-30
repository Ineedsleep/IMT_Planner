using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Services;

namespace IMT_Planner_ViewModels;

public class SuperManagerDetailsViewModel : ObservableObject
{
    
    private string _group;
    public IRelayCommand UpdateCommand { get; }


    private readonly SuperManagerSelectionService _superManagerSelectionService;
    private readonly SuperManagerRepositoryService _repositoryService;

    public SuperManagerDetailsViewModel(SuperManagerSelectionService superManagerSelectionService, SuperManagerRepositoryService repositoryService)
    {
        _superManagerSelectionService = superManagerSelectionService;
        _repositoryService = repositoryService;
        _superManagerSelectionService.SuperManagerChanged -= HandleSuperManagerSelectionChanged;
        _superManagerSelectionService.SuperManagerChanged += HandleSuperManagerSelectionChanged;
        UpdateCommand = new RelayCommand(Update);

        try
        {
            _superManagerSelectionService.CreateSuperManagerCollection(_repositoryService.GetAllSuperManagers());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }  


    
    private void HandleSuperManagerSelectionChanged(string obj)
    {
        OnPropertyChanged(obj);
        OnPropertyChanged(nameof(PEElements));
        OnPropertyChanged(nameof(NVEElements));
        OnPropertyChanged(nameof(SEElements));
        OnPropertyChanged(nameof(CurrentSuperManager));
        OnPropertyChanged(nameof(Group));
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Rarity));
        OnPropertyChanged(nameof(Area));
        OnPropertyChanged(nameof(Rank));
        OnPropertyChanged(nameof(Level));
        OnPropertyChanged(nameof(Promoted));
    }


    public ObservableCollection<SuperManagerElementViewModel> SEElements
    {
        get => _superManagerSelectionService.SEElements;
    } 

    public ObservableCollection<SuperManagerElementViewModel> NVEElements
    {
        get => _superManagerSelectionService.NVEElements;
    }  
    public ObservableCollection<SuperManagerElementViewModel> PEElements
    {
        get => _superManagerSelectionService.PEElements;
    }

    public SuperManager CurrentSuperManager
    {
        get { return _superManagerSelectionService.CurrentSuperManager; }
        set
        {
            _superManagerSelectionService.CurrentSuperManager = value;

        }
    }

    public string Group
    {
        get => _group;
        set
        {
            _group = value;
            OnPropertyChanged();
        }
    }

    public string? Name
    {
        get { return CurrentSuperManager?.Name; }
        set
        {
            if (CurrentSuperManager.Name != value)
            {
                CurrentSuperManager.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public Rarity Rarity
    {
        get => CurrentSuperManager.Rarity;
        set
        {
            if (CurrentSuperManager.Rarity != value)
            {
                CurrentSuperManager.Rarity = value;
                OnPropertyChanged(nameof(Rarity));
            }
        }
    }

    public Areas Area
    {
        get => CurrentSuperManager.Area;
        set
        {
            if (CurrentSuperManager.Area != value)
            {
                CurrentSuperManager.Area = value;
                OnPropertyChanged(nameof(Area));
            }
        }
    }

    public Rank? Rank
    {
        get => CurrentSuperManager.Rank;
        set
        {
            if (CurrentSuperManager.Rank != value)
            {
                CurrentSuperManager.Rank = value;
                OnPropertyChanged(nameof(Rank));
            }
        }
    }

    public byte Level
    {
        get => CurrentSuperManager.Level;
        set
        {
            if (CurrentSuperManager.Level != value)
            {
                CurrentSuperManager.Level = value;
                OnPropertyChanged(nameof(Level));
            }
        }
    }

    //Enum Values for Comboboxes
    public bool Promoted
    {
        get => CurrentSuperManager.Promoted;
        set
        {
            if (CurrentSuperManager.Promoted != value)
            {
                CurrentSuperManager.Promoted = value;
                OnPropertyChanged(nameof(Promoted));
            }
        }
    }
    
    public IEnumerable<Rarity> Rarities
    {
        get { return Enum.GetValues(typeof(Rarity)).Cast<Rarity>(); }
    }
    public IEnumerable<Areas> Areas
    {
        get { return Enum.GetValues(typeof(Areas)).Cast<Areas>(); }
    }
    
    //Command Methods
    private void Update()
    {
        _repositoryService.UpdateSuperManager(CurrentSuperManager);
    }
}