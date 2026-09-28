using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.Dashboard
{
    public class DahsboardVM
    {
        public int TotalBooks { get; set; }
        public int TotalCategories { get; set; }
        public int TotalAuthors { get; set; }

        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ConfirmOrders { get; set; }
        public int PackedOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancellOrders { get; set; }

        public int PendingPaymnts { get; set; }
        public int ConfirmPayments { get; set; }
        public int CancellPayments { get; set; }

        public decimal TotalRevenue { get; set; }
        public int TotalUsers { get; set; }

        public List<Decimal> MonthlyRevenue { get; set; } = new List<decimal>();
        public List<int> MonthlyOrders { get; set; } = new List<int>();
    }
}
