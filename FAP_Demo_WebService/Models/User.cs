using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCert.Models.Entity
{
    public class User
    {
        public string FullName { get; set; }
        public bool Gender { get; set; }
        public DateTime DOB { get; set; }
        public string PhoneNumber { get; set; }
        public string AcademicEmail { get; set; }
        public string RollNumber { get; set; }
        public string Ethnicity { get; set; } //Dân tộc

    }
}
