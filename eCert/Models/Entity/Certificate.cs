using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace eCert.Models.Entity
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        public string CertificateName { get; set; } = "";
        public string VerifyCode { get; set; } = ""; //Dùng vào sổ văn bằng số
        public string Url { get; set; } = "";
        public string IssuerType { get; set; } = "";
        public string IssuerName { get; set; } = "";
        public string Description { get; set; } = "";
        public string Hashing { get; set; } = "";
        public int ViewCount { get; set; } = 0;
        public DateTime DateOfIssue { get; set; }
        public DateTime DateOfExpiry { get; set; }
        public string SubjectCode { get; set; } = "";
        public string RollNumber { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Nationality { get; set; } = ""; //Quốc tịch
        public string PlaceOfBirth { get; set; } = ""; //Nơi sinh
        public string Curriculum { get; set; } = "";
        public string GraduationYear { get; set; }
        public string GraduationGrade { get; set; } = "";
        public string GraduationDecisionNumber { get; set; } = ""; //Số quyết định tốt nghiệp
        public string DiplomaNumber { get; set; } = ""; //Số hiệu văn bằng
        //Foreign key
        public int CampusId { get; set; }
        public int SignatureId { get; set; }
        //Relationship entity 
        public List<CertificateContents> CertificateContents = new List<CertificateContents>();
        public User User { get; set; }

    }
}
