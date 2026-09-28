using BookShop.Application.Interfaces;
using BookShop.Application.ViewModels.ShoppingCart;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _cartRepository;

        public ShoppingCartController(IShoppingCartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            ShoppingCart? cart;

            if (!string.IsNullOrEmpty(userId))
            {
                // Logged-in user's cart
                cart = await _cartRepository.GetCartForUserId(userId);
            }
            else
            {
                // Guest cart
                var guestId = GetOrCreateGuestId();

                cart = await _cartRepository.GetCartForClientId(guestId);
            }

            if(cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                var emptyCart = new ShoppingCartResponseVM
                {
                    Id = Guid.Empty,
                    Items = new List<ShoppingCartItemResponseVM>()
                };
                return View(emptyCart);
            }

            var cartVM = new ShoppingCartResponseVM
            {
                Id = cart.Id,
                Items = cart.CartItems.Select(item => new ShoppingCartItemResponseVM
                {
                    Id = item.Id,
                    BookId = item.Book.Id,
                    BookTitle = item.Book.Title,
                    BookImageUrl = item.Book.ImageUrl,
                    Price = item.Book.Price,
                    Quantity = item.Quantity
                }).ToList()
            };
            return View(cartVM);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid bookId, int quantity)
        {
            if (quantity <= 0)
            {
                return Json(new
                {
                    success = false,
                    message = "Quantity must be at least 1."
                });
            }

            var userId = GetUserId();

            if (userId != null)
            {
                await _cartRepository.AddUserCartItemAsync(
                    userId,
                    bookId,
                    quantity);
            }
            else
            {
                var guestId = GetOrCreateGuestId();

                await _cartRepository.AddGuestCartItemAsync(
                    guestId,
                    bookId,
                    quantity);
            }

            return Json(new
            {
                success = true,
                message = "Book added to cart successfully."
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var userId = GetUserId();

            if (!string.IsNullOrEmpty(userId))
            {
                var cart = await _cartRepository.GetCartForUserId(userId);

                var count = cart?.CartItems.Sum(x => x.Quantity) ?? 0;

                return Json(new { count });
            }

            var guestId = GetOrCreateGuestId();

            if (!string.IsNullOrEmpty(guestId))
            {
                var cart = await _cartRepository.GetCartForClientId(guestId);

                var count = cart?.CartItems.Sum(x => x.Quantity) ?? 0;

                return Json(new { count });
            }

            return Json(new { count = 0 });
        }

        [HttpPost]
        public async Task<IActionResult> IncrementQuantity(Guid bookId)
        {
            var userId = GetUserId();
            var clientId = GetOrCreateGuestId();

            var result = await _cartRepository
                .IncrementCartItemAsync(userId, clientId, bookId);

            if (!result)
            {
                TempData["error"] = "Failed to update cart.";
            }
            else
            {
                TempData["success"] = "Cart updated successfully.";
            }

            return RedirectToAction(
                "Index",
                "ShoppingCart",
                new { area = "Customer" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecrementQuantity(Guid bookId)
        {
            var userId = GetUserId();
            var clientId = userId == null ? GetOrCreateGuestId() : null;

            var result = await _cartRepository
                .DecrementCartItemAsync(userId, clientId, bookId);

            if (!result)
            {
                TempData["error"] = "Failed to update cart.";
            }
            else
            {
                TempData["success"] = "Cart updated successfully.";
            }

            return RedirectToAction(
                "Index",
                "ShoppingCart",
                new { area = "Customer" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Guid bookId)
        {
            var userId = GetUserId();
            var clientId = userId == null ? GetOrCreateGuestId() : null;

            var result = await _cartRepository
                .RemoveCartItemAsync(userId, clientId, bookId);

            if (!result)
            {
                TempData["error"] = "Failed to remove cart item.";
            }
            else
            {
                TempData["success"] = "Cart item removed successfully.";
            }

            return RedirectToAction(
                "Index",
                "ShoppingCart",
                new { area = "Customer" });
        }

        private string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private string GetOrCreateGuestId()
        {
            const string cookieName = "GuestId";

            if (Request.Cookies.TryGetValue(cookieName, out var guestId)
                && !string.IsNullOrEmpty(guestId))
            {
                return guestId;
            }

            guestId = Guid.NewGuid().ToString();

            Response.Cookies.Append(
                cookieName,
                guestId,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

            return guestId;
        }
    }
}
