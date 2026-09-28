using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? SpecialPrice { get; set; }
        public int Stock { get; set; }
        public int PageNumber { get; set; }
        public string? ISBN { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string PublicId { get; set; } = string.Empty;
        public bool IsNewArrival { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Category
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Author
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
