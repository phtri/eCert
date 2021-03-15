using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class ReportViewModel
    {
        public int ReportId { get; set; }
        [Required(ErrorMessage = "The Title field is required")]
        public string  Title { get; set; }
        [Required(ErrorMessage = "The Description field is required")]
        public string ReportContent { get; set; }
        public string Status { get; set; }
        public string CertificateName { get; set; }

        public int CertificateId { get; set; }
    }
}