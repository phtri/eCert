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
        public string PasswordHash { get; set; }
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "The phone number field is required")]
        [RegularExpression("^[0]{1}[1-9]{1}[0-9]{8}$", ErrorMessage = "The phone number is not valid")]
        public string PhoneNumber { get; set; } = "";
        //[Required(ErrorMessage = "The personal email field is required")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        public string PersonalEmail { get; set; } = "";
        [Required(ErrorMessage = "The email field is required")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@fpt.edu.vn$", ErrorMessage = "Email must be contain fpt.edu.vn")]
        public string AcademicEmail { get; set; } = "";
        public string Ethnicity { get; set; } = ""; //Dân tộc
        public string RollNumber { get; set; } = "";
        public string MemberCode { get; set; } = "";
        public RoleViewModel Role { get; set; }
        //Message
        public string ErrorMessage { get; set; } = "";
    }
}