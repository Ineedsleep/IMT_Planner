using System.Globalization;
using System.Windows.Controls;

namespace IMT_Planner.Views.WPF.Rules;

public class RangeValidationRule : ValidationRule
{
    public int Minimum { get; set; }
    public int Maximum { get; set; }

    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        int number;
        if (!int.TryParse(value.ToString(), out number))
        {
            return new ValidationResult(false, "Not a number");
        }

        if (number > Maximum || number < Minimum)
        {
            return new ValidationResult(false, $"Number not in range {Minimum}-{Maximum}");
        }

        return ValidationResult.ValidResult;
    }
}