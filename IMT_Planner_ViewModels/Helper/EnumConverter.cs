using CsvHelper;
using CsvHelper.Configuration;

namespace IMT_Planner_ViewModels.Helper;

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