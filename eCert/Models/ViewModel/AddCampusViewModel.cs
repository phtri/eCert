using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class AddCampusViewModel
    {
        [CampusValidation(ErrorMessage = "Please select Education System")]
        public int EducationSystemId { get; set; }
        [Required(ErrorMessage = "Please input Campus name")]
        public string CampusName { get; set; }
    }
}