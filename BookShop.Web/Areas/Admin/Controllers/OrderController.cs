using BookShop.Application.Interfaces;
using BookShop.Application.ViewModels.Order;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(OrderStatus? status = null , string? search = null ,
                                               int pageNumber = 1, int pageSize = 6)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var orders = await _orderRepository.GetAllOrdersAsync(status , search , pageNumber , pageSize);

            var totalCount = orders.TotalCount;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            ViewBag.Status = status;
            ViewBag.Search = search;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = pageNumber > 1;
            ViewBag.HasNextPage = pageNumber < totalPages;

            if (orders == null || !orders.Items.Any())
            {
                return View(new List<OrderResponseVM>());
            }

            var orderVMs = orders.Items.Select(order => new OrderResponseVM
            {
                Id = order.Id,
                AppUserId = userId,

                TotalItems = order.TotalItems,
                TotalPrice = order.TotalPrice,
                DeliveryFee = order.DeliveryFee,
                GrandTotal = order.GrandTotal,

                OrderDate = order.OrderDate,
                PaymentDate = order.PaymentDate,

                PaymentMethod = order.PaymentMethod,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,

                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                Street = order.Street,
                City = order.City,
                PostalCode = order.PostalCode,

                Phone1 = order.Phone1,
                Phone2 = order.Phone2,

                DeliveryInstructions = order.DeliveryInstructions
            }).ToList();

            return View(orderVMs);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid OrderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            var order = await _orderRepository.GetOrderByIdAsync(OrderId);
            if (order == null)
            {
                return NotFound();
            }

            var vm = new OrderResponseVM
            {
                Id = order.Id,
                AppUserId = userId,

                TotalItems = order.TotalItems,
                DeliveryFee = order.DeliveryFee,
                TotalPrice = order.TotalPrice,
                GrandTotal = order.GrandTotal,

                OrderDate = order.OrderDate,
                PaymentDate = order.PaymentDate,

                PaymentMethod = order.PaymentMethod,
                OrderStatus = order.OrderStatus,
                PaymentStatus = order.PaymentStatus,

                Items = order.Items.Select(item => new OrderItemResponseVM
                {
                    Id = item.Id,
                    BookId = item.BookId,
                    BookImageUrl = item.MBookImageUrl,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    BookTitle = item.BookTitle
                }).ToList(),

                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                Street = order.Street,
                City = order.City,
                PostalCode = order.PostalCode,
                Phone1 = order.Phone1,
                Phone2 = order.Phone2,
                DeliveryInstructions = order.DeliveryInstructions
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                TempData["error"] = "Order not found";
                return Redirect(nameof(Index));
            }

            await _orderRepository.UpdateOrderStatusAsync(orderId, status);
            TempData["success"] = "Order status updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
