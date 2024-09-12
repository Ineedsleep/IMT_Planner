using System.Collections.ObjectModel;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace IMT_Planner_ViewModels;

public class ChronoMineViewModel : ObservableObject
{
    public ObservableCollection<string> MineShaftCollection { get; set; } = new ObservableCollection<string>();

    public ChronoMineViewModel()
    {
        for (int i = 0; i < 50; i++)
        {
            MineShaftCollection.Add( (i+1).ToString());
        }
    }
}