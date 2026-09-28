using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Utility.Pagination
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
