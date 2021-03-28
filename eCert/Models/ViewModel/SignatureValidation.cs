using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class SignatureValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                if ((int)value == 0)
                    return new ValidationResult(ErrorMessage);
                else
                    return ValidationResult.Success;
            }
            else
                return ValidationResult.Success;
        }
    }
}