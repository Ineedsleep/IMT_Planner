using System.Windows.Controls;
using IMT_Planner.ViewModels;

namespace IMT_Planner.Views.WPF.UserControls;

public partial class ChronoPlannerControl : UserControl
{
    public ChronoPlannerControl()
    {
        InitializeComponent();
        DataContext = new ChronoPlannerViewModel();
    }
}