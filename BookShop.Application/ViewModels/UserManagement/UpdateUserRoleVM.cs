using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.ViewModels.UserManagement
{
    public class UpdateUserRoleVM
    {
        public string UserId { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
