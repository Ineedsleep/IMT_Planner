using System.Collections.ObjectModel;
using System.Windows.Input;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.GeneralViewModels;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IMT_Planner_ViewModels.ChronoViewModels;

public class ChronoMineEntityViewModel : ObservableObject
{
    private readonly ChronoSelectionService _chronoSelectionService;
 public ICommand EntitySelectCommand {get; private set;}
    public ObservableCollection<MineEntityViewModel> MineShaftCollection { get; set; } =
        new ObservableCollection<MineEntityViewModel>();

    public MineEntityViewModel Elevator { get; set; }
    public MineEntityViewModel Warehouse { get; set; }

    public ChronoMineEntityViewModel(ChronoSelectionService chronoSelectionService)
    {
        _chronoSelectionService = chronoSelectionService;
        EntitySelectCommand = new RelayCommand<MineEntityViewModel>(EntitySelected);
        InitEntities();
    }

    private void EntitySelected(MineEntityViewModel entity)
    {
        Console.WriteLine($"Selected entity area: {entity.Area}");
        Console.WriteLine($"Selected entity element: {entity.Element.ElementName}");
        _chronoSelectionService.UpdateCollections(entity.Area, entity.Element.ElementName);
    }

    private void InitEntities()
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
}