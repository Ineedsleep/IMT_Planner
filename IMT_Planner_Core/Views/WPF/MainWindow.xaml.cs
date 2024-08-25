

using System.Windows;
using IMT_Planner_ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace IMT_Planner.Views.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = ((App)Application.Current).ServiceProvider.GetService<SuperManagerListViewModel>();
    }
}