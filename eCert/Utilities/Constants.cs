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
            public static readonly string FPT = "FPT EDUCATION";
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

        public static class SaveCertificateLocation
        {
            public static readonly string BaseFolder = AppDomain.CurrentDomain.BaseDirectory + @"UploadedFiles\";
            public static readonly string BaseTempFolder = BaseFolder + @"temp\";
        }

    }
}