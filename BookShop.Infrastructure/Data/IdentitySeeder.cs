using BookShop.Domain.Entities;
using BookShop.Utility.IdentityHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Data
{
    public class IdentitySeeder
    {
        public static async Task LoadData(UserManager<AppUser> userManager,
                                          RoleManager<IdentityRole> roleManager,
                                          ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<IdentitySeeder>();
            try
            {
                if (!await roleManager.RoleExistsAsync(Roles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                }

                if (!await roleManager.RoleExistsAsync(Roles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(Roles.User));
                }

                if (!userManager.Users.Any())
                {
                    var adminUser = new AppUser
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Email = Roles.AdminEmail,
                        UserName = Roles.AdminEmail
                    };

                    var adminResult = await userManager.CreateAsync(adminUser, Roles.AdminPassword);
                    if (adminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, Roles.Admin);
                    }
                    else
                    {
                        foreach (var error in adminResult.Errors)
                        {
                            logger.LogError("Admin user creation failed. {Code} - {Description}", error.Code, error.Description);
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during seeding users and roles");
            }
        }
    }
}
