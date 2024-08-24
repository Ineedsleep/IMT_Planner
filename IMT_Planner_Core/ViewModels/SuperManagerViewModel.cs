using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IMT_Planner_DAL;
using IMT_Planner_Model;

namespace IMT_Planner.ViewModels;

public class SuperManagerViewModel : INotifyPropertyChanged
{
    public SuperManagerViewModel()
    {
        //SaveCommand = new Command(DoStuff);
    }

    public string Name { get; set; } // Implement PropertyChanged event in setters
    public Rank Rank { get; set; }    // Implement PropertyChanged event in setters
    public bool Promoted { get; set; }
    public byte Level { get; set; }
    public List<Element> Elements { get; set; }
    public Equipment Equipment { get; set; }
    public ICommand SaveCommand { get; set; }
    
    
    public void DoStuff(object obj)
    {
        using (var context = new ApplicationDbContext())
        {  
            context.Database.EnsureCreated();

            var newSuperManager = new SuperManager
            {
                Name = Name,
                Rank = Rank,
                Promoted = Promoted,
                Level = Level,
                Equipment = Equipment,
            };
            context.SuperManagers.Add(newSuperManager);
            context.SaveChanges();
        }
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
   
