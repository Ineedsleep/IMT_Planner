using System.Windows.Controls;
using IMT_ChronoPlanner.ViewModels;

namespace IMT_ChronoPlanner.Views.UserControls;

public partial class ChronoPlannerControl : UserControl
{
    public ChronoPlannerControl()
    {
        InitializeComponent();
        DataContext = new ChronoPlannerViewModel();
    }
}