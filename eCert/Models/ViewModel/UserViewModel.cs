using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace eCert.Models.ViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public bool Gender { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public string PhoneNumber { get; set; } = "";
        public string PersonalEmail { get; set; } = "";
        [Required]
        public string AcademicEmail { get; set; } = "";
        public string Ethnicity { get; set; } = ""; //Dân tộc
        public string RollNumber { get; set; } = "";
        public RoleViewModel Role { get; set; }

    }
}