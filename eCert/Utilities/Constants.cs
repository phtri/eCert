using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Utilities
{
    public static class Constants
    {
        public static class CertificateType
        {
            public static readonly string FPT = "FPT";
            public static readonly string PERSONAL = "PERSONAL";
        }

        public static class CertificateFormat
        {
            public static readonly string PDF = "PDF";
            public static readonly string LINK = "LINK";
            public static readonly string IMAGE = "IMAGE";
        }
    }
}