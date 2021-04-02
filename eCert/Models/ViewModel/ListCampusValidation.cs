using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class ListCampusValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                List<CampusViewModel> temp = (List<CampusViewModel>)value;
                if (temp.Count == 0)
                    return new ValidationResult(ErrorMessage);
                else
                    return ValidationResult.Success;
            }
            else
                return ValidationResult.Success;
        }
    }
}