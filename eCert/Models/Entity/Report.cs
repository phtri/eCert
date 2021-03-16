using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.Entity
{
    public class Report
    {
        public string Title { get; set; }
        public string ReportContent { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public int CertificateId { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}