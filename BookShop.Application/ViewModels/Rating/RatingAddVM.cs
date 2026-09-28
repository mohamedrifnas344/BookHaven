using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Application.ViewModels.Rating
{
    public class RatingAddVM
    {
        public Guid BookId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Value { get; set; }

        [StringLength(1000, ErrorMessage = "Review cannot exceed 1000 characters.")]
        public string? Review { get; set; }
    }
}
