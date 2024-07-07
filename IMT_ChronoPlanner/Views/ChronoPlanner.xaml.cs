using System.Windows.Controls;
using IMT_ChronoPlanner.ViewModels;

namespace IMT_ChronoPlanner.Views;

public partial class ChronoPlanner : UserControl
{
    public ChronoPlanner()
    {
        InitializeComponent();
        DataContext = new ChronoPlannerViewModel();
    }
}