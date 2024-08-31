using System.Windows;
using System.Windows.Controls;
using IMT_Planner_ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IMT_Planner.Views.WPF.UserControls;

public partial class SuperManagerCard : UserControl
{
    public SuperManagerCard()
    {
        InitializeComponent();
        DataContext = ((App)Application.Current).ServiceProvider.GetService<SuperManagerListViewModel>();
    }
}