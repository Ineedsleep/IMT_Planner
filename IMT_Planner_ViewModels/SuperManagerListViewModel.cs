using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace IMT_Planner_ViewModels;

public class SuperManagerListViewModel: ObservableObject
{
    private ObservableCollection<SuperManagerViewModel> _superManagerCollection;   
    private readonly SuperManagerService _superManagerService;
    public ICommand SaveCommand { get; private set; }
    public ICommand LoadCommand { get; private set; }

    public ObservableCollection<SuperManagerViewModel> SuperManagerCollection
    {
        get { return _superManagerCollection; }
        set
        {
            _superManagerCollection = value;
            OnPropertyChanged();
        }
    }

    public SuperManagerListViewModel()
    {
        SuperManagerCollection = new ObservableCollection<SuperManagerViewModel> ();
        _superManagerService = new SuperManagerService();
        LoadCommand = new RelayCommand<string>(async path => await LoadSuperManagersAsync(path));
        SaveCommand = new RelayCommand(SaveSuperManager);
        // Load SuperManagerViewModel items into SuperManagerCollection here. For example:
        // SuperManagerCollection.Add(new SuperManagerViewModel(manager1));
        // SuperManagerCollection.Add(new SuperManagerViewModel(manager2));
        // ... And so on, for each SuperManager object you have.
    }
    
    private void SaveSuperManager()
    {
        Console.WriteLine("Hello World");
        // Implement saving a SuperManager here
    }

    private async Task LoadSuperManagersAsync(string filePath)
    {
        var superManagers = await _superManagerService.LoadSuperManagersFromFileAsync(filePath);
        foreach (var superManager in superManagers)
        {
            SuperManagerCollection.Add(new SuperManagerViewModel(superManager));
        }
    }
}
