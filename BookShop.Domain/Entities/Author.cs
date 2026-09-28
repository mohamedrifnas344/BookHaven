using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
