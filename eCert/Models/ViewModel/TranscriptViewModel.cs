using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class TranscriptViewModel
    {
        [CampusValidation(ErrorMessage = "Please select Campus")]
        public int CampusId { get; set; }
        [CampusValidation(ErrorMessage = "Please select Education System")]
        public int EduSystemId { get; set; }
    }
}