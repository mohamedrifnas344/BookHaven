using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Entities
{
    public class ShoppingCartItem
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; } = 1;

        public Guid ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = null!;

        public Guid BookId { get; set; }
        public Book Book { get; set; } = null!;
    }
}
