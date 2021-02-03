using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCert.Utilities
{
    public class Pagination<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int MaxPage { get; set; }
        public List<T> PagingData { get; set; }
    }
}