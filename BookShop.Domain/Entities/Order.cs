using BookShop.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public string AppUserId { get; set; } = null!;

        public decimal TotalPrice { get; set; }
        public int TotalItems { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal GrandTotal { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? PaymentDate { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? PostalCode { get; set; }

        public string Phone1 { get; set; } = string.Empty;
        public string? Phone2 { get; set; }

        public string? DeliveryInstructions { get; set; }
        public string? SessionId { get; set; }
    }
}
