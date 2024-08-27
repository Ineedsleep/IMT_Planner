using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows.Media.TextFormatting;
using CsvHelper;
using CsvHelper.Configuration;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Helper;

namespace IMT_Planner_ViewModels.Services;

public class SuperManagerService
{
    // Holds the instance of SuperManager
    private SuperManager _superManager;

    //ToDo Probably just temporary

    public ObservableCollection<SuperManagerElementViewModel> SEElements { get; private set; }= new();
    public ObservableCollection<SuperManagerElementViewModel> NVEElements { get; private set; }= [];
    public ObservableCollection<SuperManagerElementViewModel> PEElements { get; private set; }= new();
    
    private ObservableCollection<SuperManagerCardViewModel> _superManagerCollection = new();
    private SuperManagerViewModel _selectedSuperManager;

    public SuperManager CurrentSuperManager
    {
        get
        {
            if (_superManager != null) 
                return _superManager;
            return new SuperManager();
        }
        set
        {
            _superManager = value;
            NotifySuperManagerChanged(nameof(CurrentSuperManager));
        }
    }

    public ObservableCollection<SuperManagerCardViewModel> SuperManagerCollection
    {
        get
        {
            
            return AdjustFilter(_superManagerCollection);
        }
        set
        {
            _superManagerCollection = value;
            NotifySuperManagerChanged(nameof(SuperManagerCollection));
        }
    }

    public SuperManagerViewModel SelectedSuperManager { 
        get
        {
            
            return _selectedSuperManager;
        }
        set
        {
            _selectedSuperManager = value;
            NotifySuperManagerChanged(nameof(SelectedSuperManager));
        } }

    private ObservableCollection<SuperManagerCardViewModel> AdjustFilter(ObservableCollection<SuperManagerCardViewModel> superManagerCollection)
    {
        return superManagerCollection;
    }

    // This event will be invoked whenever CurrentSuperManager is modified
    // Define event
    public event Action<string> SuperManagerChanged;
    // Define delegate
    public delegate void SuperManagerChangedHandler(object sender, EventArgs e);
    private void NotifySuperManagerChanged(string name) => SuperManagerChanged?.Invoke(name);

    // Add methods to manipulate the model here. 
    // For example, suppose SuperManager has a method to update its status
    public void UpdateName(string newName)
    {
        CurrentSuperManager.Name = newName;
    }


    private void PrepSuperManager()
    {
        var testi = new SuperManagerElementViewModel(new SuperManagerElement(CurrentSuperManager, new Element("Nature"),
            "SE"));
        var testi1 =
            new SuperManagerElementViewModel(new SuperManagerElement(CurrentSuperManager, new Element("Frost"), "SE"));
        var testi2 =
            new SuperManagerElementViewModel(new SuperManagerElement(CurrentSuperManager, new Element("Flame"), "SE"));
        var testi3 =
            new SuperManagerElementViewModel(new SuperManagerElement(CurrentSuperManager, new Element("Light"), "SE"));
        var testi4 = new SuperManagerElementViewModel(new SuperManagerElement(CurrentSuperManager, new Element("Dark"), "PE"));
        var testi5 = new SuperManagerElementViewModel(new SuperManagerElement(CurrentSuperManager, new Element("Wind"), "PE"));
        var testi6 =
            new SuperManagerElementViewModel(new SuperManagerElement(CurrentSuperManager, new Element("Sand"), "NVE"));
        var testi7 =
            new SuperManagerElementViewModel(new SuperManagerElement(CurrentSuperManager, new Element("Water"), "NVE"));

        ICollection<SuperManagerElementViewModel> elements = new List<SuperManagerElementViewModel>();
        elements.Add(testi);
        elements.Add(testi1);
        elements.Add(testi2);
        elements.Add(testi3);
        elements.Add(testi4);
        elements.Add(testi5);
        elements.Add(testi6);
        elements.Add(testi7);
        DistributeElements(elements);
    }

    private void DistributeElements(ICollection<SuperManagerElementViewModel> elements)
    {
        // Clear current collections

        // Distribute elements based on their effectiveness
        foreach (var element in elements)
        {
            switch (element.EffectivenessType)
            {
                case "SE":
                    SEElements.Add(element);
                    break;
                case "PE":
                    PEElements.Add(element);
                    break;
                case "NVE":
                    NVEElements.Add(element);
                    break;
            }
        }
    }

    public void LoadSuperManagersFromFileAsync(string filePath)
    {
        try
        {
            IEnumerable<SuperManager> testi = ReadAndParseCsv(filePath);
            ObservableCollection<SuperManagerCardViewModel> tmp = new ObservableCollection<SuperManagerCardViewModel>();
            foreach (var sm in testi)
            {
                tmp.Add(new SuperManagerCardViewModel(sm));
            }
            SuperManagerCollection = tmp;
        } 
        catch (Exception ex)
        {
            // Log the error
            // throw; // If you want to rethrow the exception to be handled in the calling code.
        }
    }

    private IEnumerable<SuperManager> ReadAndParseCsv(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<SuperManagerMap>();
            var records = csv.GetRecords<SuperManager>().ToList();
            return records;
        }
    }

    public class SuperManagerMap : ClassMap<SuperManager>
    {
        public SuperManagerMap()
        {
            Map(m => m.Rarity).Name("Rarity").TypeConverter<EnumConverter<Rarity>>();
            Map(m => m.Name).Name("Name");
            Map(m => m.Area).Name("Area").TypeConverter<EnumConverter<Areas>>();
            Map(m => m.Rank).Name("Rank").TypeConverter<RankConverter>();
            Map(m => m.Level).Name("Level");
            Map(m => m.Promoted).Name("Promoted");
        }
    }

    public void UpdateSelectedSuperManager(SuperManagerCardViewModel superManagerCardViewModel)
    {
        CurrentSuperManager = superManagerCardViewModel.SuperManager;
    }
}

public class RankConverter : CsvHelper.TypeConversion.ITypeConverter
{
    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            Rank defaultRank = new Rank(99);
            return defaultRank;
        }

        // The logic to create a Rank instance from a string
        int rankValue = int.Parse(text);
        Rank rank = new Rank(rankValue); // Assuming Rank class has a constructor that accepts an integer
        return rank;
    }

    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        Rank rank = (Rank)value;
        return
            rank.CurrentRank
                .ToString(); // Assuming Rank class has a suitable ToString() method to represent it as a string
    }
}