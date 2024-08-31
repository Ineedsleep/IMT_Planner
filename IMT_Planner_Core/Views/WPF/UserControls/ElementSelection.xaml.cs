using System.Windows;
using System.Windows.Controls;
using IMT_Planner_ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IMT_Planner.Views.WPF.UserControls;

public partial class ElementSelection : UserControl
{
    public ElementSelection()
    {
        InitializeComponent();
        DataContext = ((App)Application.Current).ServiceProvider.GetService<SMElementalListViewModel>();

    }
}