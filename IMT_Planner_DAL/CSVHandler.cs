using System.Globalization;
using System.IO;
using System.Windows.Media.TextFormatting;
using CsvHelper;
using CsvHelper.Configuration;
using IMT_Planner_DAL.Model;
using IMT_Planner_Model;

namespace IMT_Planner_DAL;

public class CSVHandler
{
    public IEnumerable<SuperManager> ReadAndParseCsv(string filePath, IEnumerable<Element> elements)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<SuperManagerImportModelMap>();
            var records = csv.GetRecords<SuperManagerImportModel>().ToList();

            var superManagers = new List<SuperManager>();
            try
            {
                foreach (var record in records)
                {
                    var superManager = new SuperManager
                    {
                        Name = record.SuperManagerName,
                        Area = record.SuperManagerArea,
                        Rarity = record.Rarity,
                        Rank = new Rank { CurrentRank = record.CurrentRank },
                        Promoted = record.Promoted,
                        Level = record.Level,
                    };
                
                    superManager.SuperManagerElements = record.Elements.Split(';')
                        .Select((e, i) =>
                        {
                            var element = elements.FirstOrDefault(element => element.Name == e) ?? elements.First();
                            return new SuperManagerElement
                            {
                                Element = element,
                                ElementId = element.ElementId,
                                EffectivenessType = superManager.Rarity switch
                                {
                                    Rarity.Common => (i < 1) ? "SE" : (i < 6) ? "PE" : "NVE",
                                    Rarity.Rare => (i < 2) ? "SE" : (i < 6) ? "PE" : "NVE",
                                    Rarity.Epic => (i < 3) ? "SE" : (i < 6) ? "PE" : "NVE",
                                    Rarity.Legendary => (i < 4) ? "SE" : (i < 6) ? "PE" : "NVE",
                                    _ => "SE" // Default value
                                }
                            };
                        }).ToList();

                    if (superManager.SuperManagerElements.Count > 4)
                    {
                        var rankRequirement = GetRankRequirement(superManager.Rarity);
                        for (int i = 0; i < rankRequirement.Length; i++)
                        {
                            superManager.SuperManagerElements.ElementAt(i).RankRequirement = rankRequirement[i];
                        }    
                    }
                
                
                    superManagers.Add(superManager);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return superManagers;
            }

            return superManagers;
        }
    }

    private static int[] GetRankRequirement(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Legendary:
                return new[] { 0, 1, 3, 5 };
            case Rarity.Epic:
                return new[] { 0, 3, 5 };
            case Rarity.Rare:
                return new[] { 0, 5 };
            case Rarity.Common:
                return new[] { 0 };
            default:
                return new[] { 0 };
        }
    }
    public void ExportToCSV(string filePath, IEnumerable<SuperManager> superManagers)
    {
        List<SuperManagerCSVExportModel> records = new List<SuperManagerCSVExportModel>();

        foreach (var superManager in superManagers)
        {
            IOrderedEnumerable<SuperManagerElement> tmp = null;
            if (superManager.SuperManagerElements != null)
            {
                tmp = superManager.SuperManagerElements.OrderBy(e =>
                {
                    switch (e.EffectivenessType + e.RankRequirement)
                    {
                        case "SE1": return 1;
                        case "SE2": return 2;
                        case "SE3": return 3;
                        case "SE4": return 4;
                        case "SE5": return 5;
                        case "PE": return 6;
                        case "NVE": return 7;
                        default: return 8; // Handle unexpected values
                    }

                    ;
                });
            }

            SuperManagerCSVExportModel model = new SuperManagerCSVExportModel
            {
                SuperManagerName = superManager.Name,
                SuperManagerArea = superManager.Area,
                Rarity = superManager.Rarity,
                //   Group = superManager.Group,
                CurrentRank = superManager.Rank.CurrentRank,
                Promoted = superManager.Promoted,
                Level = superManager.Level,
            };
            if (tmp != null)
                model.Elements = String.Join(";", tmp.Select(e => e.Element.Name));
            records.Add(model);
        }

        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
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
public class SuperManagerImportModelMap : ClassMap<SuperManagerImportModel>
{
    public SuperManagerImportModelMap() 
    {
        Map(m => m.SuperManagerName).Name("SuperManagerName");
        Map(m => m.SuperManagerArea).Name("SuperManagerArea").TypeConverter<EnumConverter<Areas>>();
        Map(m => m.CurrentRank).Name("CurrentRank");
        Map(m => m.Level).Name("Level");
        Map(m => m.Promoted).Name("Promoted");
        Map(m => m.Elements).Name("Elements");
        Map(m => m.Rarity).Name("Rarity").TypeConverter<EnumConverter<Rarity>>();
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