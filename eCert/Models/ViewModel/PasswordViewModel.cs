using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class PasswordViewModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatNewPassword { get; set; }
    }
}