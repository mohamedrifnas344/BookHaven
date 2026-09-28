using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.Author
{
    public class AuthorResponseVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
