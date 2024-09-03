using System.Windows;
using System.Windows.Controls;
using IMT_Planner_ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IMT_Planner.Views.WPF.UserControls;

public partial class FilterSelection : UserControl
{
    public FilterSelection()
    {
        InitializeComponent();
        DataContext = ((App)Application.Current).ServiceProvider.GetService<FilterViewModel>();
    }
}