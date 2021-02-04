using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace eCert.Models
{
    public class Certificate
    {
        public int CertificateID { get; set; }
        [Required(ErrorMessage = "Name is Required. It cannot be empty")]
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
