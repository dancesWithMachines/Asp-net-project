using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Scooterki.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsReserved: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext vc)
        {
            // return base.IsValid(value);
            if (value != null)
            {
                string check = (string) value;
                return check.Equals(null) ? new ValidationResult($"This scooter is reserved, cannot delete nor modify") : ValidationResult.Success;
            }
            else
                return false ? new ValidationResult($"Allright") : ValidationResult.Success;
        }
    }
}