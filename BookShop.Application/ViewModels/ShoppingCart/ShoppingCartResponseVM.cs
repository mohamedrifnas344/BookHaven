using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.ShoppingCart
{
    public class ShoppingCartResponseVM
    {
        public Guid Id { get; set; }
        public List<ShoppingCartItemResponseVM> Items { get; set; } = new();
        public decimal Total => Items.Sum(x => x.SubTotal);
        public int TotalItems => Items.Sum(x => x.Quantity);
    }
}
