using BookShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.Rating
{
    public class RatingAverageCountVM
    {
        public int Value { get; set; }
        public RatingStatus Status { get; set; }
    }
}
