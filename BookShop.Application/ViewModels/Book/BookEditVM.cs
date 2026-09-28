using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Application.ViewModels.Book
{
    public class BookEditVM
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(2000, MinimumLength = 10)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [Range(0.01, 100000)]
        public decimal? SpecialPrice { get; set; }

        [Range(0, 100)]
        public int Stock { get; set; }

        [Range(1, 10000)]
        public int PageNumber { get; set; }

        [StringLength(20)]
        public string? ISBN { get; set; }

        // Optional when editing
        public IFormFile? File { get; set; }

        public string? ExistingImageUrl { get; set; }

        public bool IsNewArrival { get; set; }
        public bool IsFeatured { get; set; }

        [Required]
        public DateTime PublishedDate { get; set; }

        [Required]
        [DisplayName("Category")]
        public Guid CategoryId { get; set; }

        [Required]
        [DisplayName("Author")]
        public Guid AuthorId { get; set; }
    }
}
