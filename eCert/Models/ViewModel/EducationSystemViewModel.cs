using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class EducationSystemViewModel
    {

        public int EducationSystemId { get; set; }
        [Required(ErrorMessage = "Please input Education name")]
        public string EducationName { get; set; } = "";
        public string LogoImage { get; set; } = "";
        //[ListCampusValidation(ErrorMessage = "Please add at least one campus")]
        public List<CampusViewModel> Campuses { get; set; } = new List<CampusViewModel>();
        public List<string> CampusNames { get; set; }
        [Required(ErrorMessage = "Please select File")]
        [FileExt(Allow = ".jpg,.png,.jpeg", ErrorMessage = "Only .jpg, png File is allowed")]
        public HttpPostedFileBase LogoImageFile { get; set; }
    }
}