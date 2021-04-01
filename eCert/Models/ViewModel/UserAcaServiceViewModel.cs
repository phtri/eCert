using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class UserAcaServiceViewModel : UserViewModel
    {
        public int EducationSystemId { get; set; }
        [CampusValidation(ErrorMessage = "Please select Campus")]
        public int CampusId { get; set; }
        public string EducationName { get; set; }
        public string CampusName { get; set; }
        public int RoleId { get; set; }
    }
}