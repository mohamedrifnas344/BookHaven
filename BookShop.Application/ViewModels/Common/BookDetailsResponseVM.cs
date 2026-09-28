using BookShop.Application.ViewModels.Book;
using BookShop.Application.ViewModels.Rating;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.Common
{
    public class BookDetailsResponseVM
    {
        public BookResponseVM Book { get; set; } = null!;
        public ICollection<BookResponseVM> SimilarBooks { get; set; } = new List<BookResponseVM>();
        public string? CurrentUserId { get; set; }
        public ICollection<RatingAverageCountVM> RatingAverageCounts { get; set; } = new List<RatingAverageCountVM>();
    }
}
