using System.Windows;
using System.Windows.Controls;
using IMT_Planner_ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IMT_Planner.Views.WPF.UserControls.ChronoTab;

public partial class ChronoMineOverview : UserControl
{
    
    
    public ChronoMineOverview()
    {
        InitializeComponent();
        DataContext = ((App)Application.Current).ServiceProvider.GetService<ChronoMineViewModel>();
    }
}