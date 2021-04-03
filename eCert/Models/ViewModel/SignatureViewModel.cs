using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class SignatureViewModel
    {
        public int SignatureId { get; set; }
        [Required(ErrorMessage = "Please input full name")]
        public string FullName { get; set; } = "";
        [Required(ErrorMessage = "Please input signatory position")]
        public string Position { get; set; } = "";
        public string ImageFile { get; set; } = "";
        [Required(ErrorMessage = "Please select image only (.jpg, .jpeg, .png)")]
        [FileExt(Allow = ".jpg,.jpeg,.png", ErrorMessage = "Please select image only (.jpg, .jpeg, .png)")]
        public HttpPostedFileBase SignatureImageFile { get; set; }
        //Foreign key
        [CampusValidation(ErrorMessage = "Please select Education System")]
        public int EducationSystemId { get; set; }
    }
}