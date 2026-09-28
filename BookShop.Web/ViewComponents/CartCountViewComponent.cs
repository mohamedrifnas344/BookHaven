using BookShop.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShop.Web.ViewComponents
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly IShoppingCartRepository _cartRepository;

        public CartCountViewComponent(IShoppingCartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int cartCount = 0;
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var claimUser = User as ClaimsPrincipal;
                var userId = claimUser?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var cart = await _cartRepository.GetCartForUserId(userId);
                    cartCount = cart?.CartItems.Sum(x => x.Quantity) ?? 0;
                }
            }
            else
            {
                // Guest user
                var clientId = Request.Cookies["GuestId"];

                if (!string.IsNullOrEmpty(clientId))
                {
                    var cart = await _cartRepository.GetCartForClientId(clientId);

                    cartCount = cart?.CartItems.Sum(x => x.Quantity) ?? 0;
                }
            }
            return View(cartCount);
        }
    }
}
