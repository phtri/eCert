using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace eCert.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        [Required(ErrorMessage = "Please enter student name.")]
        public string CertificateName { get; set; }
        public string VerifyCode { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Hashing { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public HttpPostedFileBase CertificateFile { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        //public Certificate(string certificateName, string verifyCode, string fileName, string type, string format, string description, string content, string hashing, int userId, int organizationId, DateTime created_at, DateTime updated_at)
        //{
            
        //    CertificateName = certificateName;
        //    VerifyCode = verifyCode;
        //    FileName = fileName;
        //    Type = type;
        //    Format = format;
        //    Description = description;
        //    Content = content;
        //    Hashing = hashing;
        //    UserId = userId;
        //    OrganizationId = organizationId;
        //    this.created_at = created_at;
        //    this.updated_at = updated_at;
        //}
    }
}