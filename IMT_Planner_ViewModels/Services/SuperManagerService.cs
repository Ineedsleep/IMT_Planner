using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using IMT_Planner_Model;
using IMT_Planner_ViewModels.Helper;

namespace IMT_Planner_ViewModels.Services;

public class SuperManagerService
{
    
    // Holds the instance of SuperManager
    private SuperManager _superManager;

    public SuperManager CurrentSuperManager
    {
        get => _superManager;
        set
        {
            _superManager = value;
            NotifySuperManagerChanged();
        }
    }

    // This event will be invoked whenever CurrentSuperManager is modified
    public event Action SuperManagerChanged;

    private void NotifySuperManagerChanged() => SuperManagerChanged?.Invoke();
    
    // Add methods to manipulate the model here. 
    // For example, suppose SuperManager has a method to update its status
    public void UpdateName(string newName)
    {
        _superManager.Name = newName;
        NotifySuperManagerChanged();
    }
    
    
    
    public async Task<IEnumerable<SuperManager>> LoadSuperManagersFromFileAsync(string filePath)
    {
        filePath = "C:\\Users\\Tower\\Downloads\\SM_Sheet.csv";
        try
        {
            return await Task.Run(() => ReadAndParseCsv(filePath));
        }
        catch (Exception ex)
        {
            // Log the error
            // throw; // If you want to rethrow the exception to be handled in the calling code.
        }

        return new List<SuperManager>(); // Return an empty list of super managers if an error occurred.
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
}
public class RankConverter : CsvHelper.TypeConversion.ITypeConverter
{
    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {   if (string.IsNullOrEmpty(text))
        {
            Rank defaultRank = new Rank(99);
            return defaultRank;
        }
        // The logic to create a Rank instance from a string
        int rankValue = int.Parse(text);
        Rank rank = new Rank(rankValue);  // Assuming Rank class has a constructor that accepts an integer
        return rank;
    }
    
    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        Rank rank = (Rank)value;
        return rank.CurrentRank.ToString();  // Assuming Rank class has a suitable ToString() method to represent it as a string
    }
}