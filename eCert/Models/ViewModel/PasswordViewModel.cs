using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eCert.Models.ViewModel
{
    public class PasswordViewModel
    {
        [Required(ErrorMessage = "The current password field is required")]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "The new password field is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "The confirm password field is required")]
        public string ConfirmPassword { get; set; }
        //Message
        public string ErrorMessage { get; set; } = "";
        public string SuccessMessage { get; set; } = "";
    }
}