using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string MBookImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
