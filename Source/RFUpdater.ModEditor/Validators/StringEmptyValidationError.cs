using System.Globalization;
using System.Windows.Controls;

namespace ModEditor.Validators
{
    public class StringEmptyValidationError : ValidationRule
    {
        public StringEmptyValidationError()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, "error info");
            }
            else
            {
                return ValidationResult.ValidResult;
            }
        }
    }
}