using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Entities
{
    public class ShoppingCart
    {
        public Guid Id { get; set; }

        public string? AppUserId { get; set; }
        public string? ClientId { get; set; }

        public ICollection<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();
    }
}
