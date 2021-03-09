using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public bool Gender { get; set; }
        public DateTime DOB { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string PersonalEmail { get; set; } = "";
        public string AcademicEmail { get; set; } = "";
        public string Ethnicity { get; set; } = ""; //Dân tộc
        public string RollNumber { get; set; } = "";
       
    }
}