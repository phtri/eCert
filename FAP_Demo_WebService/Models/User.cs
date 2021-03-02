﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCert.Models.Entity
{
    public class User
    {
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string LastName { get; set; } = "";
        public bool Gender { get; set; }
        public DateTime DOB { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string PersonalEmail { get; set; } = "";
        public string AcademicEmail { get; set; } = "";
        public string RollNumber { get; set; } = "";
    }
}
