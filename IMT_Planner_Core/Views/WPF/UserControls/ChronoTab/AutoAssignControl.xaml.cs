using System.Windows;
using System.Windows.Controls;
using IMT_Planner_ViewModels.ChronoViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IMT_Planner.Views.WPF.UserControls.ChronoTab;

public partial class AutoAssignControl : UserControl
{
    public AutoAssignControl()
    {
        InitializeComponent();
        DataContext = ((App)Application.Current).ServiceProvider.GetService<AutoAssignViewModel>();
    }
}