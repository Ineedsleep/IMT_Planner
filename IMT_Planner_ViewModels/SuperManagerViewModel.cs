using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using IMT_Planner_Model;

namespace IMT_Planner_ViewModels;

public class SuperManagerViewModel : ObservableObject
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
            if (SuperManager.Level != value)
            {
                SuperManager.Level = value;
                OnPropertyChanged(nameof(Level));
            }
        }
    }

    public bool Promoted
    {
        get => SuperManager.Promoted;
        set
        {
            if (SuperManager.Promoted != value)
            {
                SuperManager.Promoted = value;
                OnPropertyChanged(nameof(Promoted));
            }
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
    //ToDo: Add Elements, and more here


    public IRelayCommand UpdateCommand { get; }

    public SuperManagerViewModel()
    {
        SuperManager = new SuperManager();
        UpdateCommand = new RelayCommand(Update);
    }

    public SuperManagerViewModel(SuperManager superManager)
    {
        SuperManager = superManager;
        var testi = new SuperManagerElementViewModel(new SuperManagerElement(SuperManager, new Element("Nature"),"SE"));
        var testi1 = new SuperManagerElementViewModel(new SuperManagerElement(SuperManager, new Element("Frost"),"SE"));
        var testi2 = new SuperManagerElementViewModel(new SuperManagerElement(SuperManager, new Element("Flame"),"SE"));
        var testi3 = new SuperManagerElementViewModel(new SuperManagerElement(SuperManager, new Element("Light"),"SE"));
        var testi4 = new SuperManagerElementViewModel(new SuperManagerElement(SuperManager, new Element("Dark"),"PE"));
        var testi5 = new SuperManagerElementViewModel(new SuperManagerElement(SuperManager, new Element("Wind"),"PE"));
        var testi6 = new SuperManagerElementViewModel(new SuperManagerElement(SuperManager, new Element("Sand"),"NVE"));
        var testi7 = new SuperManagerElementViewModel(new SuperManagerElement(SuperManager, new Element("Water"),"NVE"));
            Elements.Add(testi);
            Elements.Add(testi1);
            Elements.Add(testi2);
            Elements.Add(testi3);
            Elements.Add(testi4);
            Elements.Add(testi5);
            Elements.Add(testi6);
            Elements.Add(testi7);
            DistributeElements();
        UpdateCommand = new RelayCommand(Update);
    }
    private void DistributeElements()
    {
        // Clear current collections
        SEElements.Clear();
        NVEElements.Clear();
        PEElements.Clear();

        // Distribute elements based on their effectiveness
        foreach (var element in Elements)
        {
            switch (element.EffectivenessType)
            {
                case "SE":
                    SEElements.Add(element);
                    break;
                case "PE":
                    PEElements.Add(element);
                    break;
                case "NVE":
                    NVEElements.Add(element);
                    break;
             
            }
        }
    }
    private void Update()
    {
        SuperManager.Name = "New Name";
    }
    public IEnumerable<Rarity> Rarities
    {
        get { return Enum.GetValues(typeof(Rarity)).Cast<Rarity>(); }
    }
    public IEnumerable<Areas> Areas
    {
        get { return Enum.GetValues(typeof(Areas)).Cast<Areas>(); }
    }
}