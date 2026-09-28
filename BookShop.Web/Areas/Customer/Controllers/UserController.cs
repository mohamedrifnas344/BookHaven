using BookShop.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "USER")]
    public class UserController : Controller
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var user = await _authService.UserProfileAsync(userId);
            return View(user);
        }
    }
}
