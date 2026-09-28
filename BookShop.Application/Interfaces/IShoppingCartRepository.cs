using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task AddGuestCartItemAsync(string clientId, Guid bookId, int quantity);
        Task AddUserCartItemAsync(string userId, Guid bookId, int quantity);
        Task MergeGuestCartAsync(string clientId, string userId);
        Task<ShoppingCart?> GetCartForClientId(string clientId);
        Task<ShoppingCart?> GetCartForUserId(string userId);
        Task<bool> IncrementCartItemAsync(
            string? userId,
            string? clientId,
            Guid bookId);

        Task<bool> DecrementCartItemAsync(
            string? userId,
            string? clientId,
            Guid bookId);

        Task<bool> RemoveCartItemAsync(
            string? userId,
            string? clientId,
            Guid bookId);

        Task ClearCartAsync(string userId);
    }
}
