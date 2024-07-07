using IMT_ChronoPlanner.ViewModels;

namespace IMT_ChronoPlanner;

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