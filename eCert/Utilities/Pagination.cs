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
        public List<T> PagingData { get; set; } = new List<T>();

        public Pagination<T> GetPagination(List<T> list, int pageSize, int pageNumber)
        {
            Pagination<T> pagination = new Pagination<T>();
            pagination.PageNumber = pageNumber;
            pagination.PageSize = pageSize;
            pagination.MaxPage = (int)Math.Ceiling((double)list.Count / pagination.PageSize);
            pagination.PagingData = list.Skip<T>(pageSize * (pageNumber - 1)).Take(pageSize).ToList<T>();
            return pagination;
        }

        public int CountItem()
        {
            return PagingData.Count;
        }

        public T ReturnFirstItem()
        {
            return PagingData[0];
        }
    }
}