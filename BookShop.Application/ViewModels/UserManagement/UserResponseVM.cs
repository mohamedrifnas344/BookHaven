using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.UserManagement
{
    public class UserResponseVM
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
