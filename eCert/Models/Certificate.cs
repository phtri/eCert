using System;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
=======
>>>>>>> d2e8ba75ff198e5c5aa0855daeef1d404b379405

namespace eCert.Models
{
    public class Certificate
    {
        public int CertificateID { get; set; }
        public string CertificateName { get; set; } = "";
        public string VerifyCode { get; set; } = "";
        public string FileName { get; set; } = "";
        public string Type { get; set; } = "";
        public string Format { get; set; } = "";
        public string Description { get; set; } = "";
        public string Content { get; set; } = "";
        public string Hashing { get; set; } = "";
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        //Foreign key
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        // not database entity
        public HttpPostedFileBase CertificateFile { get; set; }
    }
}
