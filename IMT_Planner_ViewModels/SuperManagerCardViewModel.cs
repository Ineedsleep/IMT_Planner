using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Services;

namespace IMT_Planner_ViewModels;

public class SuperManagerCardViewModel : ObservableObject
{
    private IMT_Planner_Model.SuperManager _superManager;
    private string _group;
    private ObservableCollection<SuperManagerElementViewModel> _elements = new ObservableCollection<SuperManagerElementViewModel>();

    public ObservableCollection<SuperManagerElementViewModel> SEElements { get; set; } =
        new ObservableCollection<SuperManagerElementViewModel>();
    public ObservableCollection<SuperManagerElementViewModel> NVEElements { get; set; }  = new ObservableCollection<SuperManagerElementViewModel>();
    public ObservableCollection<SuperManagerElementViewModel> PEElements { get; set; } = new ObservableCollection<SuperManagerElementViewModel>();
    public SuperManager SuperManager
    {
        get { return _superManager; }
        set
        {
            _superManager = value;
            OnPropertyChanged();
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
        get { return _superManager.Name; }
        set
        {
            if (_superManager.Name != value)
            {
                _superManager.Name = value;
                OnPropertyChanged();
            }
        }
    }

    public Rarity Rarity
    {
        get => SuperManager.Rarity;
        set
        {
            if (SuperManager.Rarity != value)
            {
                SuperManager.Rarity = value;
                OnPropertyChanged(nameof(Rarity));
            }
        }
    }

    public Areas Area
    {
        get => SuperManager.Area;
        set
        {
            if (SuperManager.Area != value)
            {
                SuperManager.Area = value;
                OnPropertyChanged(nameof(Area));
            }
        }
    }

    public Rank? Rank
    {
        get => SuperManager.Rank;
        set
        {
            if (SuperManager.Rank != value)
            {
                SuperManager.Rank = value;
                OnPropertyChanged(nameof(Rank));
            }
        }
    }

    public byte Level
    {
        get => SuperManager.Level;
        set
        {
                SuperManager.Level = value;
                OnPropertyChanged(nameof(Level));
        }
    }

    public bool Promoted
    {
        get => SuperManager.Promoted;
        set
        {
                SuperManager.Promoted = value;
                OnPropertyChanged(nameof(Promoted));
        }
    }

    public ObservableCollection<SuperManagerElementViewModel> Elements
    {
        get => _elements;
        set
        {
            if (Equals(value, _elements)) return;
            _elements = value;
            OnPropertyChanged();
        }
    }

    public SuperManagerCardViewModel()
    {
        SuperManager = new SuperManager();
    }

    public SuperManagerCardViewModel(SuperManager superManager,SuperManagerSelectionService _superManagerSelectionService)
    {
        SuperManager = superManager;
        _superManagerSelectionService.SuperManagerCardUpdated -= CardUpdate;
        _superManagerSelectionService.SuperManagerCardUpdated += CardUpdate;
    }

    private void CardUpdate()
    {
       OnPropertyChanged(nameof(Elements));
       OnPropertyChanged(nameof(Promoted));
       OnPropertyChanged(nameof(Level));
       OnPropertyChanged(nameof(Rank));
       OnPropertyChanged(nameof(Area));
       OnPropertyChanged(nameof(Rarity));
       OnPropertyChanged(nameof(Name));
       OnPropertyChanged(nameof(Group));
       OnPropertyChanged(nameof(SuperManager));
    }
}