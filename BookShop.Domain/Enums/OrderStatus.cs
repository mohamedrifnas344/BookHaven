using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Packed,
        Shipped,
        Delivered,
        Cancelled
    }
}
