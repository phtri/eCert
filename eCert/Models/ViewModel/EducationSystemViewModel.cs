using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class EducationSystemViewModel
    {
        public int EducationSystemId { get; set; }
        public string EducationSystemName { get; set; }
        public List<CampusViewModel> ListCampus { get; set; }
    }
}