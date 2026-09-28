using BookShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Application.ViewModels.Rating
{
    public class RatingStatusUpdateVM
    {
        public Guid Id { get; set; }

        [Required]
        public RatingStatus Status { get; set; }
    }
}
