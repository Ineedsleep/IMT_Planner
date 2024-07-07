using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Input;
using IMT_ChronoPlanner.Models;
using MVVM;

namespace IMT_ChronoPlanner.ViewModels;

public class SuperManagerViewModel : INotifyPropertyChanged
{
    public SuperManagerViewModel()
    {
        SaveCommand = new CustomCommand(DoStuff);
        Elements = new List<Element>(){Element.Dark, Element.Flame, Element.Frost};
    }

    public string Name { get; set; } // Implement PropertyChanged event in setters
    public byte Rank { get; set; }    // Implement PropertyChanged event in setters
    
    public bool Promoted { get; set; }
    public byte Level { get; set; }
    public List<Element> Elements { get; set; }
    public string Equipment { get; set; }

    public ICommand SaveCommand { get; set; }
    
    
    public void DoStuff(object obj)
    {
        using (var context = new SuperManagerContext())
        {  
            context.Database.EnsureCreated();

            var newSuperManager = new SuperManager
            {
                Name = Name,
                Rank = Rank,
                Promoted = Promoted,
                Level = Level,
                Elements =  Elements,
                Equipment = Equipment
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
   
