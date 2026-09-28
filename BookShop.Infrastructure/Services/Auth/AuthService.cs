using BookShop.Application.IServices;
using BookShop.Application.ViewModels.Account;
using BookShop.Application.ViewModels.UserManagement;
using BookShop.Domain.Entities;
using BookShop.Utility.IdentityHelper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager,
                           SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<UserResponseVM>> GetAllUsersAsync()
        {
            var users = _userManager.Users.Where(a => a.Email != Roles.AdminEmail).ToList();

            var userList = new List<UserResponseVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new UserResponseVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName ?? "N/A",
                    LastName = user.LastName ?? "N/A",
                    Email = user.Email ?? "N/A",
                    Role = roles.FirstOrDefault() ?? "N/A",
                });
            }

            return userList;
        }

        public async Task<int> GetUserCountAsync()
        {
            return await _userManager.Users.CountAsync();
        }

        public async Task<AuthResult> Login(LoginRequestVM requestVM)
        {
            var user = await _userManager.FindByEmailAsync(requestVM.Email);
            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Emain or Password is incorrect. please check and try again."
                };
            }

            var userResult = await _signInManager.PasswordSignInAsync(user, requestVM.Password,
                                                                      isPersistent: true, lockoutOnFailure: false);
            if (userResult.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return new AuthResult
                {
                    Success = true,
                    Message = "Login successful",
                    Roles = roles
                };
            }

            return new AuthResult
            {
                Success = false,
                Message = "Email or Password is in correct. please check and tyr again."
            };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AuthResult> Register(RegisterRequestVM requestVM)
        {
            var user = await _userManager.FindByEmailAsync(requestVM.Email);
            if (user != null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Email is already in use"
                };
            }

            var identityUser = new AppUser
            {
                FirstName = requestVM.FirstName,
                LastName = requestVM.LastName,
                Email = requestVM.Email,
                UserName = requestVM.Email
            };

            var identityResult = await _userManager.CreateAsync(identityUser, requestVM.Password);
            if (!identityResult.Succeeded)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Registration failed"
                };
            }

            var roleResult = await _userManager.AddToRoleAsync(identityUser, Roles.User);
            if (!roleResult.Succeeded)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Role assignment failed"
                };
            }

            return new AuthResult
            {
                Success = true,
                Message = "Registration completed successfully"
            };
        }

        public async Task<bool> UpdateUserRoleAsync(UpdateUserRoleVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return false;
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            if (currentRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!removeResult.Succeeded)
                {
                    return false;
                }
            }

            var addResult = await _userManager.AddToRoleAsync(user, model.Role);
            return addResult.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, roles);
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<UserResponseVM> UserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            var roles = await _userManager.GetRolesAsync(user);

            return new UserResponseVM
            {
                Id = user.Id,
                FirstName = user.FirstName ?? "N/A",
                LastName = user.LastName ?? "N/A",
                Email = user.Email ?? "N/A",
                Role = roles.FirstOrDefault() ?? "N/A",
            };
        }
    }
}
