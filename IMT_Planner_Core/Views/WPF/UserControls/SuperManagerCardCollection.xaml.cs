using System.Windows;
using System.Windows.Controls;
using IMT_Planner_ViewModels;
using IMT_Planner_ViewModels.MainViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IMT_Planner.Views.WPF.UserControls;

public partial class SuperManagerCardCollection : UserControl
{
    public SuperManagerCardCollection()
    {
        InitializeComponent();
        DataContext = ((App)Application.Current).ServiceProvider.GetService<SuperManagerListViewModel>();
    }
}