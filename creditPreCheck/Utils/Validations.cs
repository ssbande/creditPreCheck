using System;
using System.ComponentModel.DataAnnotations;

namespace creditPreCheck.Utils
{
    public class FutureDateAttribute: ValidationAttribute
    {
        public override bool IsValid(object value) => value != null
            && (DateTime)value < DateTime.Today;
    }

    public class OldestDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value) => value != null
              && (DateTime)value > DateTime.Parse(Config.oldestDate);
    }
}
