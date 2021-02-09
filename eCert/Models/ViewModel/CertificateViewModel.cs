using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class CertificateViewModel
    {
        public int CertificateId { get; set; }
        public string CertificateName { get; set; } = "";
        public string VerifyCode { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string Format { get; set; } = "";
        public string Description { get; set; } = "";
        public int ViewCount { get; set; } = 0;
        //not database entity
        public HttpPostedFileBase[] CertificateFile { get; set; }
        //Foreign table column
        public string Content { get; set; }

    }
}