using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.Entity
{
    public class Staff : User
    {
        public int EducationSystemId { get; set; }
        public int CampusId { get; set; }
        public string EducationName { get; set; }
        public string CampusName { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}