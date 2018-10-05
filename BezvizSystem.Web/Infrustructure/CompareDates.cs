using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BezvizSystem.Web.Infrustructure
{
    public class LessThanOtherDate : CompareAttribute
    {
        private readonly string _other;

        public LessThanOtherDate(string other)    
            :base(other)
        {
            _other = other;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_other);

            if (property == null)
            {
                return new ValidationResult(
                    string.Format("Unknown property: {0}", _other)
                );
            }

            var otherValue = property.GetValue(validationContext.ObjectInstance, null);

            if (value == null || otherValue == null) return null;

            if (((DateTime)value).CompareTo((DateTime)otherValue) <= 0)
                return null;
            else return new ValidationResult(ErrorMessage);
        }
    }

    public class MoreThanOtherDate : CompareAttribute
    {
        private readonly string _other;

        public MoreThanOtherDate(string other)
            : base(other)
        {
            _other = other;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_other);

            if (property == null)
            {
                return new ValidationResult(
                    string.Format("Unknown property: {0}", _other)
                );
            }

            var otherValue = property.GetValue(validationContext.ObjectInstance, null);

            if (value == null || otherValue == null) return null;

            if (((DateTime)value).CompareTo((DateTime)otherValue) >= 0)
                return null;
            else return new ValidationResult(ErrorMessage);
        }
    }
}