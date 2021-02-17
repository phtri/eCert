using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class CertificateContentViewModel
    {
        public string Content { get; set; } = "";
        public string Format { get; set; } = "";
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}