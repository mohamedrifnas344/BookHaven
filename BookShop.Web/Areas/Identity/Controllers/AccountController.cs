using BookShop.Application.Interfaces;
using BookShop.Application.IServices;
using BookShop.Application.ViewModels.Account;
using BookShop.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShop.Web.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IShoppingCartRepository _cartRepository;

        public AccountController(IAuthService authService ,
                                 IShoppingCartRepository cartRepository)
        {
            _authService = authService;
            _cartRepository = cartRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestVM requestVM)
        {
            if (!ModelState.IsValid)
            {
                return View(requestVM);
            }

            var result = await _authService.Register(requestVM);
            if (!result.Success)
            {
                TempData["error"] = result.Message ?? "Registration failed.";
                return View(requestVM);
            }

            TempData["success"] = result.Message ?? "Registration success";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            var model = new LoginRequestVM
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestVM requestVM)
        {
            if (!ModelState.IsValid)
            {
                return View(requestVM);
            }

            var result = await _authService.Login(requestVM);
            if (!result.Success)
            {
                TempData["error"] = result.Message ?? "Login failed.";
                return View(requestVM);
            }

            // Get logged-in user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get guest cart ID
            var guestId = Request.Cookies["GuestId"];

            // Merge guest cart into user cart
            if (!string.IsNullOrEmpty(guestId) && !string.IsNullOrEmpty(userId))
            {
                await _cartRepository.MergeGuestCartAsync(guestId, userId);

                Response.Cookies.Delete("GuestId", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
            }

            TempData["success"] = result.Message ?? "Login successful";

            // Handle Return URL Redirects first
            if (!string.IsNullOrEmpty(requestVM.ReturnUrl) && Url.IsLocalUrl(requestVM.ReturnUrl))
            {
                return Redirect(requestVM.ReturnUrl);
            }

            if (result.Roles?.Contains("ADMIN") == true)
            {
                return RedirectToAction(nameof(Index), "Dashboard", new { area = "Admin" });
            }

            return RedirectToAction(nameof(Index), "Home", new { area = "Customer" });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            TempData["success"] = "Logged out successfully";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
