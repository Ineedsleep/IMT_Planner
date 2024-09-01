using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using IMT_Planner_Model;

namespace IMT_Planner_DAL;

public class CSVHandler
{
    
    
    public IEnumerable<SuperManager> ReadAndParseCsv(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<SuperManagerMap>();
            var records = csv.GetRecords<SuperManager>().ToList();
            return records;
        }
    }


    public void ExportToCSV(string filePath, IEnumerable<SuperManager> superManagers)
    {
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(superManagers);
        }
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



public class EnumConverter<T> : CsvHelper.TypeConversion.ITypeConverter where T : struct
{
    public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrEmpty(text))
        {
            return default(T);
        }

        if (Enum.TryParse<T>(text, out var parsedEnum))
        {
            return parsedEnum;
        }

        if (int.TryParse(text, out var parsedInt))
        {
            return (T)Enum.ToObject(typeof(T), parsedInt);
        }

        throw new Exception();
    }

    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        return value?.ToString();
    }
}