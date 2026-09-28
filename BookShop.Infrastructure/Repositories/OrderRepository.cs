using BookShop.Application.Interfaces;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Infrastructure.Data;
using BookShop.Utility.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PageResult<Order>> GetAllOrdersAsync(OrderStatus? status = null , string? search = null ,
                                                                int pageNumber = 1, int pageSize = 6)
        {
            var query = _dbContext.Orders
                .Include(q => q.Items)
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable();

            // Search by first name, last name, or full name
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(o =>
                    o.FirstName.Contains(search) ||
                    o.LastName.Contains(search) ||
                    (o.FirstName + " " + o.LastName).Contains(search));
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.OrderStatus == status.Value);
            }

            //Pagination
            var skipResult = (pageNumber - 1) * pageSize;

            var totalCount = await query.CountAsync();
            var items = await query.Skip(skipResult).Take(pageSize).ToListAsync();

            var pageResult = new PageResult<Order>()
            {
                TotalCount = totalCount,
                Items = items
            };
            return pageResult;
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _dbContext.Orders
                .Include(q => q.Items).FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId , OrderStatus? status = null)
        {
            var query = _dbContext.Orders
                .Include(q => q.Items)
                .Where(x => x.AppUserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .AsQueryable();
            if (status.HasValue)
            {
                query = query.Where(o => o.OrderStatus == status.Value);
            }
            return await query.ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                return;
            }
            order.OrderStatus = status;

            // COD payment is completed when the order is delivered
            if (order.PaymentMethod == PaymentMethod.CashOnDelivery)
            {
                if (status == OrderStatus.Delivered)
                {
                    // COD payment completed on delivery
                    order.PaymentStatus = PaymentStatus.Success;
                    order.PaymentDate = DateTime.UtcNow;
                }
                else
                {
                    // COD payment is still pending before delivery
                    order.PaymentStatus = PaymentStatus.Pending;
                    order.PaymentDate = null;
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePaymentStatusAsync(Guid orderId, PaymentStatus status)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                return;
            }
            order.PaymentStatus = status;
            if (status == PaymentStatus.Success)
            {
                order.PaymentDate = DateTime.UtcNow;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateStripeSessionIdAsync(Guid orderId, string sessionId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                return;
            }
            order.SessionId = sessionId;
            await _dbContext.SaveChangesAsync();
        }
    }
}
