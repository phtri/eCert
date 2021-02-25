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
        public string Format { get; set; } = "";
        public int CertificateId { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}