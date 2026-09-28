using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.Order
{
    public class OrderItemResponseVM
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string BookImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Price * Quantity;
    }
}
