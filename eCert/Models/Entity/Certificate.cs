﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace eCert.Models.Entity
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        public string CertificateName { get; set; } = "";
        public string VerifyCode { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string Format { get; set; } = "";
        public string Description { get; set; } = "";
        public string Hashing { get; set; } = "";
        public int ViewCount { get; set; } = 0;
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        //Foreign key
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        
    }
}
