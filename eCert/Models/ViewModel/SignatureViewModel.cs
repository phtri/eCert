using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class SignatureViewModel
    {
        public int SignatureId { get; set; }
        public string FullName { get; set; } = "";
        public string Position { get; set; } = "";
        public string ImageFile { get; set; } = "";
        public HttpPostedFileBase SignatureImageFile { get; set; }
        //Foreign key
        public int EducationSystemId { get; set; }
    }
}