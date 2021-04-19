using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.Entity
{
    public class Report
    {
        public int ReportId { get; set; }
        public string CertificateName { get; set; }
        public string Title { get; set; }
        public string ReportContent { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public int CertificateId { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
        public string Fullname { get; set; }
        public string Rollnumber { get; set; }

    }
}