using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.Entity
{
    public class Signature
    {
        public int SignatureId { get; set; }
        public string FullName { get; set; } = "";
        public string Position { get; set; } = "";
        public string ImageFile { get; set; } = "";
        //Foreign key
        public int EducationSystemId { get; set; }
        public string EducationName { get; set; }
    }
}