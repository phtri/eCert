using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        //Foreign key
        public int UserId { get; set; }
    }
}