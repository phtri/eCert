using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.Entity
{
    public class Campus
    {
        public int CampusId { get; set; }
        public string CampusName { get; set; } = "";
        //Foreign key
        public int EducationSystemId { get; set; }
    }
}