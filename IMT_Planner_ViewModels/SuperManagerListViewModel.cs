using System.Collections.ObjectModel;
using System.Windows.Input;
using IMT_Planner_ViewModels.Services;
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
    
    public SuperManagerListViewModel(SuperManagerService smService)
    {
        _superManagerService = smService;
        _superManagerCollection = new ObservableCollection<SuperManagerViewModel> ();
        LoadCommand = new RelayCommand<string>(async path => await LoadSuperManagersAsync(path));
        SaveCommand = new RelayCommand(SaveSuperManager);
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
