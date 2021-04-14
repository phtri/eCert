using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models.Entity
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}