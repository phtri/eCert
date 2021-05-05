using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class SubjectViewModel
    {
        public string Semester { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public string StudentFullName { get; set; }
        public float Mark { get; set; }
        //If transcript is generated to certificate in the database
        public bool IsGenerated { get; set; } = false;
        //Link to certificate
        public string Link { get; set; }
        public int CampusId { get; set; }
        public int SignatureId { get; set; }
    }
}