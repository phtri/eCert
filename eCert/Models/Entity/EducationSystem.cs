using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.Entity
{
    public class EducationSystem
    {
        public int EducationSystemId { get; set; }
        public string EducationName { get; set; }
        public List<Campus> ListCampus { get; set; }

    }
}