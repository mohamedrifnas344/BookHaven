using BookShop.Application.ViewModels.Rating;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.Book
{
    public class BookResponseVM
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
        public bool IsNew { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime PublishedDate { get; set; }

        public string CategoryName { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;

        public ICollection<RatingResponseVM> RatingResponse { get; set; } = new List<RatingResponseVM>();
        public ICollection<RatingAverageCountVM> RatingAverageCounts { get; set; } = new List<RatingAverageCountVM>();

    }
}
