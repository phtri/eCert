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
       
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "The phone number field is required")]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression("^[0]{1}[1-9]{9}$", ErrorMessage = "The phone number is not valid")]
        public string PhoneNumber { get; set; } = "";
        public string PersonalEmail { get; set; } = "";
        [Required(ErrorMessage = "The email field is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@fpt.edu.vn$", ErrorMessage = "Email must be contain fpt.edu.vn")]
        public string AcademicEmail { get; set; } = "";
        public string Ethnicity { get; set; } = ""; //Dân tộc
        public string RollNumber { get; set; } = "";
        public RoleViewModel Role { get; set; }

    }
}