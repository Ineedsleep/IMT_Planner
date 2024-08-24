using IMT_Planner.ViewModels;

namespace IMT_Planner.Views.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext =new  SuperManagerViewModel();
    }
}