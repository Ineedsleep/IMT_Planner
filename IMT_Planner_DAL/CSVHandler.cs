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
    private Dictionary<string, PassiveAttributeName> _passiveAttributeNames;

    public IEnumerable<SuperManager> ReadAndParseCsv(string filePath, IEnumerable<Element> elements,
        IEnumerable<PassiveAttributeName> passiveAttributeNames)
    {
        _passiveAttributeNames =
            passiveAttributeNames.ToDictionary(p => p.Abbreviation, StringComparer.OrdinalIgnoreCase);
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<SuperManagerImportModelMap>();
            var importModels = csv.GetRecords<SuperManagerImportModel>().ToList();

            var superManagers = new List<SuperManager>();

            foreach (var im in importModels)
            {
                var superManager = new SuperManager
                {
                    Name = im.SuperManagerName,
                    Rarity = im.Rarity,
                    Area = im.SuperManagerArea,
                    Rank = new Rank { CurrentRank = im.CurrentRank },
                    Level = im.Level,
                    Promoted = im.Promoted,
                    Passives = ParsePassives(im.Passives),
                    //   Priority = im.Priority
                };
                superManager.SuperManagerElements = im.Elements.Split(';')
                    .Select((e, i) =>
                    {
                        var element = elements.FirstOrDefault(element => element.Name == e) ?? elements.First();
                        return new SuperManagerElement
                        {
                            SuperManager = superManager,
                            SuperManagerId = superManager.SuperManagerId,
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

                foreach (var passive in superManager.Passives)
                {
                    passive.SuperManager = superManager;
                    passive.SuperManagerId = superManager.SuperManagerId;
                }
                superManagers.Add(superManager);
            }

            return superManagers;
        }
    }

    public ICollection<Passive> ParsePassives(string passivesString)
    {
        var passives = new List<Passive>();
        var parsedPassives = passivesString.Split(';'); // Assuming passives are separated by semicolons in the string

        foreach (var passiveData in parsedPassives)
        {
            var fields = passiveData.Split(','); // Assuming fields are separated by commas within each passive
            if (fields.Length == 3 && _passiveAttributeNames.ContainsKey(fields[0]))
            {
                var abbreviation = fields[0].Trim();
                var attributeValueString = fields[1].Trim();
                var rankRequirement = int.Parse(fields[2], CultureInfo.InvariantCulture);

                double? attributeValue = null;
                if (!string.Equals(attributeValueString, "null", StringComparison.OrdinalIgnoreCase))
                {
                    attributeValue = double.Parse(attributeValueString, CultureInfo.InvariantCulture);
                }

                var passiveAttribute = _passiveAttributeNames[abbreviation];

                var passive = new Passive
                {
                    PassiveAttributeNameId = passiveAttribute.Id,
                    AttributeValue = attributeValue,
                    RankRequirement = rankRequirement,
                    Name = passiveAttribute,
                };
                passives.Add(passive);
            }
        }

        return passives;
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
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<SuperManagerImportModelMap>();
            // Write the header for the CSV file
            csv.WriteHeader<SuperManagerImportModel>();
            csv.NextRecord();
            foreach (var superManager in superManagers)
            {
                var importModel = new SuperManagerImportModel
                {
                    SuperManagerName = superManager.Name,
                    Rarity = superManager.Rarity,
                    SuperManagerArea = superManager.Area,
                    CurrentRank = superManager.Rank.CurrentRank,
                    Level = superManager.Level,
                    Promoted = superManager.Promoted,
                    Elements = StringifyElements(superManager.SuperManagerElements),
                    Passives = StringifyPassives(superManager.Passives), // Using the StringifyPassives method
                    //  Priority = superManager.Priority
                };
                csv.WriteRecord(importModel);
                csv.NextRecord();
            }
        }
    }

    private string StringifyPassives(ICollection<Passive> passives)
    {
        return string.Join(";", passives.Select(p => $"{p.Name.Abbreviation},{p.AttributeValue},{p.RankRequirement}"));
    }

    private string StringifyElements(ICollection<SuperManagerElement>? superManagerSuperManagerElements)
    {
        IOrderedEnumerable<SuperManagerElement> tmp = null;
        if (superManagerSuperManagerElements != null)
        {
            tmp = superManagerSuperManagerElements.OrderBy(e =>
            {
                string switchTrigger = e.EffectivenessType +
                                       (e.EffectivenessType == "SE" && e.RankRequirement != 0
                                           ? e.RankRequirement.ToString()
                                           : "");
                switch (switchTrigger)
                {
                    case "SE": return 1;
                    case "SE1": return 2;
                    case "SE2": return 3;
                    case "SE3": return 4;
                    case "SE4": return 5;
                    case "SE5": return 6;
                    case "PE": return 7;
                    case "NVE": return 8;
                    default: return 9; // Handle unexpected values
                }
            });
            return String.Join(";", tmp.Select(e => e.Element.Name));
        }

        return String.Empty;
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
            Map(m => m.Passives).Name("Passives"); // Add this mapping
            // Add other necessary mappings
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
}