using System.Threading.Channels;
using System.Windows.Input;
using IMT_Planner_ViewModels.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IMT_Planner_ViewModels.ChronoViewModels;

public class AutoAssignViewModel : ObservableObject
{
    private readonly ChronoSelectionService _chronoSelectionService;

    public ICommand AutoAssignCommand { get; private set; }
    public AutoAssignViewModel(ChronoSelectionService chronoSelectionService)
    {
        _chronoSelectionService = chronoSelectionService;
        AutoAssignCommand = new RelayCommand(Assign);
    }

    private void Assign()
    {
        _chronoSelectionService.AutoAssignSuperManager();
    }
}