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

 public ObservableCollection<MineEntityViewModel> MineShaftCollection => _chronoSelectionService.MineShaftCollection;
 public MineEntityViewModel Elevator => _chronoSelectionService.Elevator;
 public MineEntityViewModel Warehouse => _chronoSelectionService.Warehouse;
    public ChronoMineEntityViewModel(ChronoSelectionService chronoSelectionService)
    {
        _chronoSelectionService = chronoSelectionService;
        EntitySelectCommand = new RelayCommand<MineEntityViewModel>(EntitySelected);
        _chronoSelectionService.InitEntities();
    }

    private void EntitySelected(MineEntityViewModel entity)
    {
        _chronoSelectionService.UpdateCollections(entity.Area, entity.Element.ElementName);
    }

   
}