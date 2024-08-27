using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Services;

namespace IMT_Planner_ViewModels;

public class SuperManagerViewModel : ObservableObject
{
    
    private string _group;
    public IRelayCommand UpdateCommand { get; }


    private readonly SuperManagerService _superManagerService;
    public SuperManagerViewModel(SuperManagerService superManagerService)
    {
        _superManagerService = superManagerService;
        _superManagerService.SuperManagerChanged -= HandleSuperManagerChanged;
        _superManagerService.SuperManagerChanged += HandleSuperManagerChanged;
        UpdateCommand = new RelayCommand(Update);
    }  


    
    private void HandleSuperManagerChanged(string obj)
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
        get => _superManagerService.SEElements;
    } 

    public ObservableCollection<SuperManagerElementViewModel> NVEElements
    {
        get => _superManagerService.NVEElements;
    }  
    public ObservableCollection<SuperManagerElementViewModel> PEElements
    {
        get => _superManagerService.PEElements;
    }

    public SuperManager CurrentSuperManager
    {
        get { return _superManagerService.CurrentSuperManager; }
        set
        {
            _superManagerService.CurrentSuperManager = value;

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
        CurrentSuperManager.Name = "New Name";
    }
}