using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Utility.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId , OrderStatus? status = null);
        Task UpdateStripeSessionIdAsync(Guid orderId, string sessionId);
        Task UpdatePaymentStatusAsync(Guid orderId, PaymentStatus status);
        Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
        Task<PageResult<Order>> GetAllOrdersAsync(OrderStatus? status = null , string? search = null ,
                                                   int pageNumber = 1, int pageSize = 6);
    }
}
