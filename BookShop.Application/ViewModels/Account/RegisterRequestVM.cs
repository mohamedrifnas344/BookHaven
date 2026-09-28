using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Application.ViewModels.Account
{
    public class RegisterRequestVM
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "You must provide a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password length must be between 6 nad 30")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Password length must be between 6 nad 30")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
