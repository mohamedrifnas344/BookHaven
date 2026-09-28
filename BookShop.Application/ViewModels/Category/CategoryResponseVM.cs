using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.Category
{
    public class CategoryResponseVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
