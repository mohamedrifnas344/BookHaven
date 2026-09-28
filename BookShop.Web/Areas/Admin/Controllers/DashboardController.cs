using BookShop.Application.Interfaces;
using BookShop.Application.IServices;
using BookShop.Application.ViewModels.Dashboard;
using BookShop.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class DashboardController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthService _authService;

        public DashboardController(ICategoryRepository categoryRepository,
                                   IBookRepository bookRepository,
                                   IAuthorRepository authorRepository,
                                   IOrderRepository orderRepository,
                                   IAuthService authService)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _orderRepository = orderRepository;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            var today = DateTime.Now;
            var monthlyRevenue = new List<decimal>();
            var monthlyOrders = new List<int>();

            for (int i = 5; i >= 0; i--)
            {
                var target = today.AddMonths(-i);
                var monthlyData = orders.Items.Where(o => o.OrderDate.Month == target.Month &&
                                               o.OrderDate.Year == target.Year);
                monthlyRevenue.Add(monthlyData.Sum(o => o.GrandTotal));
                monthlyOrders.Add(monthlyData.Count());
            }

            var totalBooks = await _bookRepository.CountAsync();
            var totalCategories = await _categoryRepository.CountAsync();
            var totalAuthors = await _authorRepository.CountAsync();
            var totalOrders = orders.Items.Count();

            var pendingOrders = orders.Items.Count(o => o.OrderStatus == OrderStatus.Pending);
            var confirmOrders = orders.Items.Count(o => o.OrderStatus == OrderStatus.Confirmed);
            var packedOrders = orders.Items.Count(o => o.OrderStatus == OrderStatus.Packed);
            var shippedOrders = orders.Items.Count(o => o.OrderStatus == OrderStatus.Shipped);
            var deliveredOrders = orders.Items.Count(o => o.OrderStatus == OrderStatus.Delivered);
            var cancellOrders = orders.Items.Count(o => o.OrderStatus == OrderStatus.Cancelled);

            var pendingPayments = orders.Items.Count(o => o.PaymentStatus == PaymentStatus.Pending);
            var confirmPayments = orders.Items.Count(o => o.PaymentStatus == PaymentStatus.Success);
            var failPayments = orders.Items.Count(o => o.PaymentStatus == PaymentStatus.Failed);

            var totalRevenu = orders.Items.Sum(o => o.GrandTotal);
            var totalUsers = await _authService.GetUserCountAsync();

            var dashboardVM = new DahsboardVM
            {
                TotalBooks = totalBooks,
                TotalCategories = totalCategories,
                TotalAuthors = totalAuthors,
                TotalOrders = totalOrders,

                PendingOrders = pendingOrders,
                ConfirmOrders = confirmOrders,
                PackedOrders = packedOrders,
                ShippedOrders = shippedOrders,
                DeliveredOrders = deliveredOrders,
                CancellOrders = cancellOrders,

                PendingPaymnts = pendingPayments,
                ConfirmPayments = confirmPayments,
                CancellPayments = failPayments,

                TotalRevenue = totalRevenu,
                TotalUsers = totalUsers,

                MonthlyRevenue = monthlyRevenue,
                MonthlyOrders = monthlyOrders
            };

            return View(dashboardVM);
        }
    }
}
