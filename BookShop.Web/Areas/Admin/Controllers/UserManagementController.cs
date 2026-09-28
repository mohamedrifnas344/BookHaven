using BookShop.Application.IServices;
using BookShop.Application.ViewModels.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class UserManagementController : Controller
    {
        private readonly IAuthService _authService;

        public UserManagementController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _authService.GetAllUsersAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateUserRoleVM model)
        {
            var result = await _authService.UpdateUserRoleAsync(model);

            if (result)
            {
                TempData["Success"] = "Role updated successfully";
            }
            else
            {
                TempData["Error"] = "Something went wrong";
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await _authService.DeleteUserAsync(userId);

            if (result)
                TempData["success"] = "User deleted successfully";
            else
                TempData["error"] = "User not found or delete failed";

            return RedirectToAction(nameof(Index));
        }
    }
}
