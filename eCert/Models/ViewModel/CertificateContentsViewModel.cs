using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class CertificateContentsViewModel
    {
        public int CertificateContentId { get; set; }
        public string Content { get; set; } 
        public string CertificateFormat { get; set; } 
    }
}