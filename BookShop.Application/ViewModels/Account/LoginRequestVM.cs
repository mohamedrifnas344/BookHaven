using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Application.ViewModels.Account
{
    public class LoginRequestVM
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "You must provide a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password length must be between 6 nad 30")]
        public string Password { get; set; } = string.Empty;
        public string? ReturnUrl { get; set; }
    }
}
