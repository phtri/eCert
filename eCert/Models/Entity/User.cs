using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCert.Models.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public string PasswordHash { get; set; } = "";
        public bool Gender { get; set; }
        public DateTime DOB { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string PersonalEmail { get; set; } = "";
        public string AcademicEmail { get; set; } = "";
        public string Ethnicity { get; set; } = ""; //Dân tộc
        public string RollNumber { get; set; } = "";
        public string MemberCode { get; set; } = "";
        public bool IsActive { get; set; }
        //Relationship entity
        public Role Role { get; set; }
    }
}
