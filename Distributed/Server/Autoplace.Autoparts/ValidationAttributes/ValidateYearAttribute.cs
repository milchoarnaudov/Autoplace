using System.ComponentModel.DataAnnotations;

namespace Autoplace.Autoparts.ValidationAttributes
{
    public class ValidateYearAttribute : ValidationAttribute
    {
        private int minYear;

        public ValidateYearAttribute(int minYear)
        {
            this.minYear = minYear;
        }

        public override bool IsValid(object? value)
        {
            if (value is int year)
            {
                if (year >= minYear && year <= DateTime.UtcNow.Year)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
