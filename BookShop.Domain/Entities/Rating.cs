using BookShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Entities
{
    public class Rating
    {
        public Guid Id { get; set; }
        public int Value { get; set; } // 1 - 5
        public string? Review { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public RatingStatus Status { get; set; } = RatingStatus.Pending;

        // Book
        public Guid BookId { get; set; }
        public Book Book { get; set; } = null!;

        // User
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
    }
}
