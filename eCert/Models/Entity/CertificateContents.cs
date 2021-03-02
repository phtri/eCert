using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.Entity
{
    public class CertificateContents
    {
        public int CertificateContentId { get; set; }
        public string Content { get; set; } = "";
        public string CertificateFormat { get; set; } = "";
        public int CertificateId { get; set; }
    }
}