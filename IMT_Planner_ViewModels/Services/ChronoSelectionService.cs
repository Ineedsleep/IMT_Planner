using System.Collections.ObjectModel;
using System.Xml.Linq;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.MainViewModels;

namespace IMT_Planner_ViewModels.Services;

public class ChronoSelectionService
{
    public ObservableCollection<SuperManagerCardViewModel> UnlockedSuperManagerCards { get; set; } =
        new ObservableCollection<SuperManagerCardViewModel>();

    public ObservableCollection<SuperManagerCardViewModel> UnlockedSESuperManagerCards { get; set; } =
        new ObservableCollection<SuperManagerCardViewModel>();
    public ObservableCollection<SuperManagerCardViewModel> UnlockedPESuperManagerCards { get; set; } =
        new ObservableCollection<SuperManagerCardViewModel>();
    public ObservableCollection<SuperManagerCardViewModel> UnlockedNVESuperManagerCards { get; set; } =
        new ObservableCollection<SuperManagerCardViewModel>();
    private List<SuperManagerCardViewModel> _backupCollection { get; set; }
    public event Action EntityChanged;
    private void NotifySuperManagerCardUpdate() => EntityChanged?.Invoke();

    public ChronoSelectionService()
    {
    }

    public void UpdateCollections(Areas entityArea, string elementElementName)
    {
        // Clear the existing collection
        UnlockedSESuperManagerCards.Clear();
        UnlockedPESuperManagerCards.Clear();
        UnlockedNVESuperManagerCards.Clear();
        // Filter the backup collection based on the given area and element name
        var filteredSuperManagers = _backupCollection
            .Where(sm => sm.Area == entityArea)
            .ToList();

        // Add the filtered super managers to the unlocked collection
        foreach (var sm in filteredSuperManagers)
        {
            if (sm.SEElements.Any(x => x.EffectivenessType == "SE" && x.Name == elementElementName))
                UnlockedSESuperManagerCards.Add(sm);
            else if (sm.PEElements.Any(x => x.EffectivenessType == "PE" && x.Name == elementElementName))
                UnlockedPESuperManagerCards.Add(sm);
            else
                UnlockedNVESuperManagerCards.Add(sm);
        }

        NotifySuperManagerCardUpdate();
    }

    public void CreateChronoSmList(ICollection<SuperManagerCardViewModel> collection)
    {
        foreach (var sm in collection.Where(x => x.Unlocked).ToList())
        {
            UnlockedSuperManagerCards.Add(sm);
        }

        _backupCollection = UnlockedSuperManagerCards.ToList();
    }
}