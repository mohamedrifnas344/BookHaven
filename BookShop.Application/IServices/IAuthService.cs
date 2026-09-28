using BookShop.Application.ViewModels.Account;
using BookShop.Application.ViewModels.UserManagement;
using BookShop.Utility.IdentityHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.IServices
{
    public interface IAuthService
    {
        Task<AuthResult> Register(RegisterRequestVM requestVM);
        Task<AuthResult> Login(LoginRequestVM requestVM);
        Task Logout();
        Task<IEnumerable<UserResponseVM>> GetAllUsersAsync();
        Task<int> GetUserCountAsync();
        Task<bool> UpdateUserRoleAsync(UpdateUserRoleVM model);
        Task<bool> DeleteUserAsync(string userId);
        Task<UserResponseVM> UserProfileAsync(string userId);
    }
}
