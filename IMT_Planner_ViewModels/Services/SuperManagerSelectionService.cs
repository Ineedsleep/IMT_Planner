using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using IMT_Planner_DAL;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.GeneralViewModels;
using IMT_Planner_ViewModels.MainViewModels;

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
    private ICollection<SuperManager>? _backupManagerCollection;
    private List<Element> ElementCollection = new List<Element>();
    private List<PassiveAttributeName> PassiveNameCollection = new List<PassiveAttributeName>();

    public SuperManagerSelectionService(CSVHandler csvHandler)
    {
        _csvHandler = csvHandler;
    }

    public SuperManager CurrentSuperManager
    {
        get => _superManager;
        set
        {
            _superManager = value;
            NotifySuperManagerChanged(nameof(CurrentSuperManager));
        }
    }

    public ObservableCollection<SuperManagerCardViewModel> SuperManagerCollection
    {
        get
        {
            _superManagerCollection = AdjustFilter(_superManagerCollection);
            if (_superManagerCollection.Count == 0)
                return _superManagerCollection;
            _superManagerCollection = SortCollection(_superManagerCollection);
            return _superManagerCollection;
        }
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

    public void CreatePassiveNameCollection(IEnumerable<PassiveAttributeName> passiveNames)
    {
        foreach (var element in passiveNames)
        {
            PassiveNameCollection.Add(element);
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

    private static int[] GetRankRequirement(Rarity? rarity)
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


    private Tuple<int, int, int> GetElementCount(Rarity? rarity)
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
            tmp.Add(new SuperManagerElementViewModel(ele));
        }

        foreach (var ele in CurrentSuperManager.SuperManagerElements.Where(element =>
                     element.EffectivenessType == "PE"))
        {
            PEElements.Add(new SuperManagerElementViewModel(ele));
        }

        foreach (var ele in CurrentSuperManager.SuperManagerElements.Where(
                     element => element.EffectivenessType == "NVE"))
        {
            NVEElements.Add(new SuperManagerElementViewModel(ele));
        }

        SEElements =
            new ObservableCollection<SuperManagerElementViewModel>(tmp.OrderBy(e => e.RankRequirement).ToList());
        ElementsChanged();
    }

    // This event will be invoked whenever CurrentSuperManager is modified
    // Define event
    public event Action<string> SuperManagerChanged;
    public event Action SuperManagerCardUpdated;
    public event Action ElementsChanged;
    public event Action FilterChanged;
    public event Action CollectionChanged;

    // Define delegate
    public delegate void SuperManagerChangedHandler(object sender, EventArgs e);

    private void NotifySuperManagerChanged(string name) => SuperManagerChanged?.Invoke(name);
    private void NotifySuperManagerCardUpdate() => SuperManagerCardUpdated?.Invoke();
    private void NotifyElementsUpdate() => ElementsChanged?.Invoke();
    private void NotifyFilterUpdate() => FilterChanged?.Invoke();
    private void CollectionUpdate() => CollectionChanged?.Invoke();

    public void UpdateCard(SuperManager superManager)
    {
        var targetVM = _superManagerCollection.SingleOrDefault(vm => vm.SuperManager == superManager);
        if (targetVM != null)
        {
            NotifySuperManagerCardUpdate();
        }
    }

    public void LoadSuperManagersFromFileAsync(string filePath)
    {
        try
        {
            IEnumerable<SuperManager> csvContent =
                _csvHandler.ReadAndParseCsv(filePath, ElementCollection, PassiveNameCollection);
            SuperManagerCollection.Clear();
            foreach (var sm in csvContent)
            {
                SuperManagerCollection.Add(new SuperManagerCardViewModel(sm, this));
            }

            _backupManagerCollection = SuperManagerCollection.Select(sm => sm.SuperManager).ToList();

        }
        catch (Exception ex)
        {
            if (Debugger.IsAttached)
                throw;
        }
        finally
        {
            CollectionUpdate();
            NotifyFilterUpdate();
        }
    }


    public void UpdateSelectedSuperManager(SuperManagerCardViewModel superManagerCardViewModel)
    {
        CurrentSuperManager = superManagerCardViewModel.SuperManager;
        SplitElements();
    }

    public void ExportToCSV(string filePath)
    {
        IEnumerable<SuperManager> superManagers = _superManagerCollection.Select(sm => sm.SuperManager);
        _csvHandler.ExportToCSV(filePath, superManagers);
    }

    public void ApplyFilters(Expression<Func<SuperManager, bool>> filterExpression)
    {
        if (_backupManagerCollection != null)
        {
            var filteredSuperManagers = _backupManagerCollection.AsQueryable()
                .Where(filterExpression)
                .Select(sm => new SuperManagerCardViewModel(sm, this))
                .ToList();
            SuperManagerCollection.Clear();
            foreach (var smv in filteredSuperManagers)
            {
                SuperManagerCollection.Add(smv);
            }
        }

        NotifyFilterUpdate();
    }

    public ObservableCollection<SuperManagerCardViewModel> SortCollection(
        ObservableCollection<SuperManagerCardViewModel> collection)
    {
        var tmp = collection.ToList();

        tmp = tmp
            .OrderBy(x => x.Rarity)
            .ThenBy(x => x.Unlocked)
            .ToList();
        tmp = tmp
            .OrderBy(sm => sm.Unlocked == false)
            .ThenByDescending(sm => sm.Rarity)
            .ToList();
        ObservableCollection<SuperManagerCardViewModel> result =
            new ObservableCollection<SuperManagerCardViewModel>(tmp);

        return result;
    }

    public IOrderedEnumerable<string> GetAllTags()
    {
        List<string> tags = new List<string>();
        foreach (SuperManagerCardViewModel smvm in SuperManagerCollection)
        {
            tags.AddRange(smvm.Tags);
        }

        var result = tags.Distinct().OrderBy(tag => tag);
        return result;
    }
}