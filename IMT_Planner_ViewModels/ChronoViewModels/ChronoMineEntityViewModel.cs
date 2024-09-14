using System.Collections.ObjectModel;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.GeneralViewModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels.ChronoViewModels;

public class ChronoMineEntityViewModel : ObservableObject
{
    public ObservableCollection<MineEntityViewModel> MineShaftCollection { get; set; } =
        new ObservableCollection<MineEntityViewModel>();

    public MineEntityViewModel Elevator { get; set; }
    public MineEntityViewModel Warehouse { get; set; }

    public ChronoMineEntityViewModel()
    {
        InitEntities();
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