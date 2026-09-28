using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.Rating
{
    public class RatingResponseVM
    {
        public Guid Id { get; set; }
        public int Value { get; set; }
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; }

        public RatingStatus Status { get; set; }

        public Guid BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;

        public string AppUserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
