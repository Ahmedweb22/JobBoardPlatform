using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.Common
{
    public class PaginationResult<T>
{
        public IEnumerable<T> Date { get; set; } = new List<T>();
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }

        public PaginationResult()
        {
        }
        public PaginationResult(IEnumerable<T> data, int page, int pageSize, int totalItems)
        {
            Date = data;
            Page = page;
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            TotalItems = totalItems;
        }
    }
}
