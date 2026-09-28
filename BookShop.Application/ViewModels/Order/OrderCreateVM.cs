using BookShop.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Application.ViewModels.Order
{
    public class OrderCreateVM
    {
        [ValidateNever]
        public ICollection<OrderItemCreateVM> Items { get; set; } = new List<OrderItemCreateVM>();

        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
        public int TotalItems => Items.Sum(x => x.Quantity);
        public decimal DeliveryFee => 4m;
        public decimal GrandTotal => TotalPrice + DeliveryFee;

        public PaymentMethod PaymentMethod { get; set; }


        [Required(ErrorMessage = "First name is required")]
        [StringLength(100)]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100)]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [StringLength(100)]
        public string? PostalCode { get; set; }

        [Required]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        public string Phone1 { get; set; } = string.Empty;

        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        public string? Phone2 { get; set; }

        [StringLength(300)]
        public string? DeliveryInstructions { get; set; }
    }
}
