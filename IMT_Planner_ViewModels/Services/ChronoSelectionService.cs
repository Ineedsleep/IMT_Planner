using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Linq;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.GeneralViewModels;
using IMT_Planner_ViewModels.MainViewModels;

namespace IMT_Planner_ViewModels.Services;

public class ChronoSelectionService
{
    public ObservableCollection<SuperManagerCardViewModel> _unlockedSuperManagerCards =
        new ObservableCollection<SuperManagerCardViewModel>();

    public ObservableCollection<SuperManagerCardViewModel> UnlockedSESuperManagerCards { get; set; } =
        new ObservableCollection<SuperManagerCardViewModel>();
    public ObservableCollection<SuperManagerCardViewModel> UnlockedPESuperManagerCards { get; set; } =
        new ObservableCollection<SuperManagerCardViewModel>();
    public ObservableCollection<SuperManagerCardViewModel> UnlockedNVESuperManagerCards { get; set; } =
        new ObservableCollection<SuperManagerCardViewModel>();
    
    public ObservableCollection<MineEntityViewModel> MineShaftCollection { get; set; } =
        new ObservableCollection<MineEntityViewModel>();
    public ObservableCollection<ElementViewModel> ElementCollection { get; set; } =
    new ObservableCollection<ElementViewModel>();
    public MineEntityViewModel Elevator { get; set; }
    public MineEntityViewModel Warehouse { get; set; }
    public int SelectedPattern { get; set; } = 6;

    public event Action EntityChanged;
    private void NotifySuperManagerCardUpdate() => EntityChanged?.Invoke();

    public ChronoSelectionService()
    {
    
        MineShaftCollection.CollectionChanged += MineShaftCollection_CollectionChanged;
    }
    
    
    public void UpdateCollections(Areas entityArea, string elementElementName)
    {
        // Clear the existing collection
        UnlockedSESuperManagerCards.Clear();
        UnlockedPESuperManagerCards.Clear();
        UnlockedNVESuperManagerCards.Clear();
        // Filter the backup collection based on the given area and element name
        var filteredSuperManagers = _unlockedSuperManagerCards
            .Where(sm => sm.Area == entityArea)
            .ToList();

        // Add the filtered super managers to the unlocked collection
        foreach (var sm in filteredSuperManagers)
        {
            if (sm.SEElements.Any(x => x.EffectivenessType == "SE" && x.Name == elementElementName && x.Element.RankRequirement <= sm.CurrentRank))
                UnlockedSESuperManagerCards.Add(sm);
            else if (sm.PEElements.Any(x => x.EffectivenessType == "PE" && x.Name == elementElementName ||  (x.EffectivenessType == "SE" && x.Name == elementElementName && x.Element.RankRequirement >= sm.CurrentRank)))
                UnlockedPESuperManagerCards.Add(sm);
            else
                UnlockedNVESuperManagerCards.Add(sm);
        }

        NotifySuperManagerCardUpdate();
    }
    public void InitEntities()
    {
        for (int i = 0; i < 50; i++)
        {
            MineEntityViewModel mineEntity = new MineEntityViewModel(new MineEntity())
            {
                Number = i + 1, Area = Areas.Mineshaft, OpeningCost = 0, MaxCost = 1000,
                Element = 
                    new ElementViewModel(new Element() { Name = $"Flame" })
            };
            MineShaftCollection.Add(mineEntity);
        }

        Elevator = new MineEntityViewModel(new MineEntity())
        {
            Area = Areas.Elevator, OpeningCost = 0, MaxCost = 101,
            Element = 
                new ElementViewModel(new Element() { Name = $"Nature" })
        };
        Warehouse = new MineEntityViewModel(new MineEntity())
        {
            Area = Areas.Warehouse, OpeningCost = 0, MaxCost = 102,
            Element = 
                new ElementViewModel(new Element() { Name = $"Water" })
        };
        
    }
    public void CreateChronoSmList(ICollection<SuperManagerCardViewModel> collection)
    {
        foreach (var sm in collection.Where(x => x.Unlocked).ToList())
        {
            _unlockedSuperManagerCards.Add(sm);
        }
    }
    
    
    private void MineShaftCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (MineEntityViewModel item in e.NewItems)
                {
                    // Handle new items
                    if(item.Number < 9)
                        SubscribeToItemChanges(item);
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                foreach (MineEntityViewModel item in e.OldItems)
                {
                    // Handle removed items
                    UnsubscribeFromItemChanges(item);
                }
                break;

            // Handle other actions as needed (e.g., Replace, Move, Reset)

            default:
                break;
        }
    }
    
    private void SubscribeToItemChanges(MineEntityViewModel item)
    {
        item.PropertyChanged += Item_PropertyChanged;
    }

    private void UnsubscribeFromItemChanges(MineEntityViewModel item)
    {
        item.PropertyChanged -= Item_PropertyChanged;
    }

    private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        var item = sender as MineEntityViewModel;
        var element = item.Element;
        //pattern -1 other is pattern
        for (int i = item.Number + SelectedPattern-1; i < MineShaftCollection.Count;i = i + SelectedPattern)
        {
            MineShaftCollection.ElementAt(i).Element = element;
        }
        // Handle property changes for individual items
        // For example, update related items if certain properties change
    }
}