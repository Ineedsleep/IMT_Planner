using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IMT_ChronoPlanner.Models;

namespace IMT_ChronoPlanner.ViewModels;

public class ChronoPlannerViewModel  : INotifyPropertyChanged
{
    private ObservableCollection<MineEntity> _mineShafts;
    
    public ObservableCollection<MineEntity> MineShafts
    {
        get => _mineShafts;
        set
        {
            _mineShafts = value;
            OnPropertyChanged(nameof(MineShafts));
        }
    }

    public ChronoPlannerViewModel()
    {
        MineShafts = LoadEntitiesFromDatabase();
    }
    
    private ObservableCollection<MineEntity> LoadEntitiesFromDatabase()
    {
        // Implement your logic here to load MineEntity objects from your database and return them as an ObservableCollection
      var testi = new ObservableCollection<MineEntity>();
      for (int i = 0; i < 50; i++)
      {
          testi.Add(new MineEntity(i +1,1,0,0,Element.Nature, new SuperManager("Sam_" + i.ToString())));
      }
      return testi;
    }

    
    
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}