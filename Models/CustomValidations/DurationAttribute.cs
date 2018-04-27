using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Models.CustomValidations
{
    public class DurationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime now = DateTime.Now;
            if( value != null)
            {
                var timeDiff = DateTime.Compare((DateTime) value, now);
                if (timeDiff < 0)
                {
                    //
                    return new ValidationResult("Date must be set in the future");
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Date must be set in the future");
        }
    }
}