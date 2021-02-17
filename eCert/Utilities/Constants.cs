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
        }

        public static class SaveCertificateLocation
        {
            public static readonly string BaseFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/UploadedFiles/";
            //Location save personal certiifcates
            public static readonly string PersonalImgFolder = "/Personal/Imgs/";
            public static readonly string PersonalPdfFolder = "/Personal/Pdfs/";
            public static readonly string PersonalLinkFile = "/Personal/links.txt";
            //Location save fU education certificates
            public static readonly string FuImgFolder = "/FU/Imgs/";
            public static readonly string FuPdfFolder = "/FU/Pdfs/";
        }

    }
}