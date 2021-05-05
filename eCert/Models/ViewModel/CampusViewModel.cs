using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class CampusViewModel
    {
        public int CampusId { get; set; }
        [Required(ErrorMessage = "Please input Campus name")]
        public string CampusName { get; set; }
        [CampusValidation(ErrorMessage = "Please select Education System")]
        public int EducationSystemId { get; set; }
    }
}