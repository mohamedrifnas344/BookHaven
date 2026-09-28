using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Application.ViewModels.Category
{
    public class CategoryAddVM
    {
        [Required]
        [StringLength(50 , MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;
    }
}
