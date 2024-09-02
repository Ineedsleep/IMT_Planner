using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Media.TextFormatting;
using CsvHelper;
using CsvHelper.Configuration;
using IMT_Planner_DAL;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Helper;
using LinqKit;

namespace IMT_Planner_ViewModels.Services;

public class SuperManagerSelectionService
{
    private readonly CSVHandler _csvHandler;

    // Holds the instance of SuperManager
    private SuperManager _superManager;

    public ObservableCollection<SuperManagerElementViewModel> SEElements { get; set; } =
        new ObservableCollection<SuperManagerElementViewModel>();

    public ObservableCollection<SuperManagerElementViewModel> NVEElements { get; set; } =
        new ObservableCollection<SuperManagerElementViewModel>();

    public ObservableCollection<SuperManagerElementViewModel> PEElements { get; set; } =
        new ObservableCollection<SuperManagerElementViewModel>();

    private ObservableCollection<SuperManagerCardViewModel> _superManagerCollection = new();
    private ICollection<SuperManager> _backupManagerCollection;
    private List<Element> ElementCollection = new List<Element>();

    public SuperManagerSelectionService(CSVHandler csvHandler)
    {
        _csvHandler = csvHandler;
    }

    public SuperManager CurrentSuperManager
    {
        get
        {
            if (_superManager != null)
                return _superManager;
            return new SuperManager();
        }
        set
        {
            _superManager = value;
            NotifySuperManagerChanged(nameof(CurrentSuperManager));
        }
    }

    public ObservableCollection<SuperManagerCardViewModel> SuperManagerCollection
    {
        get { return AdjustFilter(_superManagerCollection); }
        set
        {
            _superManagerCollection = value;
            NotifySuperManagerChanged(nameof(SuperManagerCollection));
        }
    }
    
    private ObservableCollection<SuperManagerCardViewModel> AdjustFilter(
        ObservableCollection<SuperManagerCardViewModel> superManagerCollection)
    {
        return superManagerCollection;
    }

    public void CreateSuperManagerCollection(IEnumerable<SuperManager> collection)
    {
        foreach (var sm in collection)
        {
            CheckInvalidElements(sm);
            SuperManagerCollection.Add(new SuperManagerCardViewModel(sm, this));
            _backupManagerCollection = SuperManagerCollection.Select(sm => sm.SuperManager).ToList();
        }
    }
    public void CreateElementCollection(IEnumerable<Element> allElements)
    {
        foreach (var element in allElements)
        {
            ElementCollection.Add(element);
        }
    }
    private void CheckInvalidElements(SuperManager sm)
    {
        if (sm.SuperManagerElements == null)
        {
            var elementCount = GetElementCount(sm.Rarity);

            sm.SuperManagerElements = new List<SuperManagerElement>();
            int counter = 0;
            foreach (var item in CreateElements(sm, "SE", elementCount.Item1, sm.SuperManagerElements.Count))
                sm.SuperManagerElements.Add(item);
            foreach (var item in CreateElements(sm, "PE", elementCount.Item2, sm.SuperManagerElements.Count))
                sm.SuperManagerElements.Add(item);
            foreach (var item in CreateElements(sm, "NVE", elementCount.Item3, sm.SuperManagerElements.Count))
                sm.SuperManagerElements.Add(item);
        }
    }

    public IEnumerable<SuperManagerElement> CreateElements(SuperManager sm, string effectivenessType, int count,
        int counter)
    {
        if (effectivenessType == "SE")
        {
            int[] rankRequirement = GetRankRequirement(sm.Rarity);


            for (var i = 0; i < count; i++)
            {
                yield return new SuperManagerElement
                {
                    SuperManager = sm,
                    SuperManagerId = sm.SuperManagerId,
                    EffectivenessType = effectivenessType,
                    Element = ElementCollection[counter],
                    RankRequirement = rankRequirement[i]
                };
                counter++;
            }
        }
        else
        {
            
            for (var i = 0; i < count; i++)
            {
                yield return new SuperManagerElement
                {
                    SuperManager = sm,
                    SuperManagerId = sm.SuperManagerId,
                    EffectivenessType = effectivenessType, Element = ElementCollection[counter],
                };
                counter++;
            }
        }
    }

    static string[] elements = new string[]
    {
        "Nature",
        "Frost",
        "Flame",
        "Light",
        "Dark",
        "Wind",
        "Sand",
        "Water"
    };

    private static int[] GetRankRequirement(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Legendary:
                return new[] { 0, 1, 3, 5 };
            case Rarity.Epic:
                return new[] { 0, 3, 5 };
            case Rarity.Rare:
                return new[] { 0, 5 };
            case Rarity.Common:
                return new[] { 0 };
            default:
                return new[] { 0 };
        }
    }


    private Tuple<int, int, int> GetElementCount(Rarity rarity)
    {
        int counter = 0;
        Tuple<int, int, int> elementCount;
        switch (rarity)
        {
            case Rarity.Legendary:
                return new Tuple<int, int, int>(4, 2, 2);
            case Rarity.Epic:
                return new Tuple<int, int, int>(3, 3, 2);
            case Rarity.Rare:
                return new Tuple<int, int, int>(2, 4, 2);
            case Rarity.Common:
            default:
                return new Tuple<int, int, int>(1, 5, 2);
        }
    }

    
    private void SplitElements()
    {
        SEElements.Clear();
        PEElements.Clear();
        NVEElements.Clear();
        IList<SuperManagerElementViewModel> tmp = new List<SuperManagerElementViewModel>();
        foreach (var ele in CurrentSuperManager.SuperManagerElements.Where(element =>
                     element.EffectivenessType == "SE"))
        {
            tmp.Add(new SuperManagerElementViewModel(ele, SEElements.Count));
        }

        foreach (var ele in CurrentSuperManager.SuperManagerElements.Where(element =>
                     element.EffectivenessType == "PE"))
        {
            PEElements.Add(new SuperManagerElementViewModel(ele, PEElements.Count));
        }

        foreach (var ele in CurrentSuperManager.SuperManagerElements.Where(
                     element => element.EffectivenessType == "NVE"))
        {
            NVEElements.Add(new SuperManagerElementViewModel(ele, NVEElements.Count));
        }
        SEElements = new ObservableCollection<SuperManagerElementViewModel>(tmp.OrderBy(e => e.RankRequirement).ToList());
        ElementsChanged();
    }

    // This event will be invoked whenever CurrentSuperManager is modified
    // Define event
    public event Action<string> SuperManagerChanged;
    public event Action SuperManagerCardUpdated;

    public event Action ElementsChanged;

    // Define delegate
    public delegate void SuperManagerChangedHandler(object sender, EventArgs e);

    private void NotifySuperManagerChanged(string name) => SuperManagerChanged?.Invoke(name);
    private void NotifySuperManagerCardUpdate() => SuperManagerCardUpdated?.Invoke();
    private void NotifyElementsUpdate() => ElementsChanged?.Invoke();

    // Add methods to manipulate the model here. 
    // For example, suppose SuperManager has a method to update its status
    public void UpdateName(string newName)
    {
        CurrentSuperManager.Name = newName;
    }

    public void UpdateCard(SuperManager superManager)
    {
        var targetVM = _superManagerCollection.SingleOrDefault(vm => vm.SuperManager == superManager);

        if (targetVM != null)
        {
            // Modify the VM properties based on provided SuperManager properties.
            // Here just as an example, `Area` property is updated.
            NotifySuperManagerCardUpdate();
        }
        else
        {
            // Handle the scenario when no such SuperManagerCardViewModel can be found.
        }
    }

    public void LoadSuperManagersFromFileAsync(string filePath)
    {
        try
        {
            IEnumerable<SuperManager> csvContent = _csvHandler.ReadAndParseCsv(filePath,ElementCollection);
            SuperManagerCollection.Clear();
            foreach (var sm in csvContent)
            {
                SuperManagerCollection.Add(new SuperManagerCardViewModel(sm, this));
            }

            _backupManagerCollection = SuperManagerCollection.Select(sm => sm.SuperManager).ToList();
        }
        catch (Exception ex)
        {
            // Log the error
            // throw; // If you want to rethrow the exception to be handled in the calling code.
        }
    }
    

    public void UpdateSelectedSuperManager(SuperManagerCardViewModel superManagerCardViewModel)
    {
        CurrentSuperManager = superManagerCardViewModel.SuperManager;
        SplitElements();
    }

    public void ExportToCSV(string filePath)
    {
        IEnumerable <SuperManager> superManagers= _superManagerCollection.Select(sm => sm.SuperManager);
        _csvHandler.ExportToCSV(filePath, superManagers);
    }

    public void ApplyFilters(Expression<Func<SuperManager,bool>> filterExpression)
    {
            var filteredSuperManagers = _backupManagerCollection.AsQueryable()
                                                    .Where(filterExpression)
                                                    .Select(sm => new SuperManagerCardViewModel(sm,this))
                                                    .ToList();
        SuperManagerCollection.Clear();
        foreach(var smv in filteredSuperManagers)
        {
            SuperManagerCollection.Add(smv);
        }  

    }
}
    

