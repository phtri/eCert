using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Utilities
{
    public static class Constants
    {
        public static class CertificateIssuer
        {
            public static readonly string FPT = "FPT";
            public static readonly string PERSONAL = "PERSONAL";
        }

        public static class CertificateFormat
        {
            public static readonly string PDF = "PDF";
            public static readonly string LINK = "LINK";
            public static readonly string PNG = "PNG";
            public static readonly string JPG = "JPG";
            public static readonly string JPEG = "JPEG";
            public static readonly string ZIP = "ZIP";
        }

        public static class SaveLocation
        {
            public static readonly string Base = AppDomain.CurrentDomain.BaseDirectory + @"UploadedFiles\";
            public static readonly string BaseCertificateFolder = Base + @"Certificates\";
            public static readonly string BaseTempFolder = Base + @"temp\";
            public static readonly string EducationSystemLogoImageFolder = Base + @"EducationSystemLogoImage\";
            public static readonly string EducationSystemSignatureImageFolder = Base + @"EducationSignatureImage\";

            public static readonly string BaseTemplateFileCert = AppDomain.CurrentDomain.BaseDirectory + @"TemplateFiles\Certificate_Template.xlsx";
            public static readonly string BaseTemplateFileDiploma = AppDomain.CurrentDomain.BaseDirectory + @"TemplateFiles\Diploma_Template.xlsx";
            
        }

        public static class Role
        {
            public static readonly int OWNER = 1;
            public static readonly int ADMIN = 2;
            public static readonly int FPT_UNIVERSITY_ACADEMIC = 3;
            public static readonly int SUPER_ADMIN = 4;
        }

        public static class TypeImportExcel
        {
            public static readonly int IMPORT_CERT = 1;
            public static readonly int IMPORT_DIPLOMA = 2;
        }

        public static class StatusReport
        {
            public static readonly string PENDING = "Pending";
            public static readonly string UPDATED = "Updated";
            public static readonly string REJECTED = "Rejected";
        }


    }
}