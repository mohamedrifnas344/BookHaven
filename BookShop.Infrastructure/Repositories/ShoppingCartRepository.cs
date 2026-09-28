using BookShop.Application.Interfaces;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingCartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddGuestCartItemAsync(
            string clientId,
            Guid bookId,
            int quantity)
        {
            var cart = await _dbContext.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    Id = Guid.NewGuid(),
                    ClientId = clientId,
                    CartItems = new List<ShoppingCartItem>()
                };

                _dbContext.ShoppingCarts.Add(cart);
            }

            var existingItem = cart.CartItems
                .FirstOrDefault(x => x.BookId == bookId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new ShoppingCartItem
                {
                    Id = Guid.NewGuid(),
                    BookId = bookId,
                    ShoppingCartId = cart.Id,
                    Quantity = quantity
                };

                _dbContext.ShoppingCartItems.Add(newItem);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUserCartItemAsync(
        string userId,
        Guid bookId,
        int quantity)
        {
            var cart = await _dbContext.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.AppUserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    Id = Guid.NewGuid(),
                    AppUserId = userId
                };

                _dbContext.ShoppingCarts.Add(cart);

                await _dbContext.SaveChangesAsync();
            }

            var existingItem = cart.CartItems
                .FirstOrDefault(x => x.BookId == bookId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new ShoppingCartItem
                {
                    Id = Guid.NewGuid(),
                    ShoppingCartId = cart.Id,
                    BookId = bookId,
                    Quantity = quantity
                };

                _dbContext.ShoppingCartItems.Add(cartItem);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task MergeGuestCartAsync(string clientId, string userId)
        {
            var guestCart = await _dbContext.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (guestCart == null || !guestCart.CartItems.Any())
                return;

            var userCart = await _dbContext.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.AppUserId == userId);

            if (userCart == null)
            {
                // Reassign guest cart to user
                guestCart.AppUserId = userId;
                guestCart.ClientId = null;
            }
            else
            {
                foreach (var guestItem in guestCart.CartItems.ToList())
                {
                    var existingUserItem = userCart.CartItems
                        .FirstOrDefault(x => x.BookId == guestItem.BookId);

                    if (existingUserItem != null)
                    {
                        existingUserItem.Quantity += guestItem.Quantity;
                    }
                    else
                    {
                        // Reassign instead of recreating
                        guestItem.ShoppingCartId = userCart.Id;
                        userCart.CartItems.Add(guestItem);
                    }
                }

                // Remove the now-empty guest cart
                _dbContext.ShoppingCarts.Remove(guestCart);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ShoppingCart?> GetCartForClientId(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new Exception("GuestId is null or empty");

            return await _dbContext.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(m => m.Book)
                .FirstOrDefaultAsync(x => x.ClientId == clientId);
        }

        public async Task<ShoppingCart?> GetCartForUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("UserId is null or empty");

            return await _dbContext.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(m => m.Book)
                .FirstOrDefaultAsync(x => x.AppUserId == userId);
        }

        public async Task<bool> IncrementCartItemAsync(
            string? userId,
            string? clientId,
            Guid bookId)
        {
            var cartItem = await _dbContext.ShoppingCartItems
                .Include(x => x.ShoppingCart)
                .FirstOrDefaultAsync(x =>
                    x.BookId == bookId &&
                    (
                        (userId != null && x.ShoppingCart.AppUserId == userId) ||
                        (clientId != null && x.ShoppingCart.ClientId == clientId)
                    ));

            if (cartItem == null)
            {
                return false;
            }

            cartItem.Quantity++;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DecrementCartItemAsync(
            string? userId,
            string? clientId,
            Guid bookId)
        {
            var cartItem = await _dbContext.ShoppingCartItems
                .Include(x => x.ShoppingCart)
                .FirstOrDefaultAsync(x =>
                    x.BookId == bookId &&
                    (
                        (userId != null && x.ShoppingCart.AppUserId == userId) ||
                        (clientId != null && x.ShoppingCart.ClientId == clientId)
                    ));

            if (cartItem == null)
            {
                return false;
            }

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else
            {
                _dbContext.ShoppingCartItems.Remove(cartItem);
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCartItemAsync(
            string? userId,
            string? clientId,
            Guid bookId)
        {
            var cartItem = await _dbContext.ShoppingCartItems
                .Include(x => x.ShoppingCart)
                .FirstOrDefaultAsync(x =>
                    x.BookId == bookId &&
                    (
                        (userId != null && x.ShoppingCart.AppUserId == userId) ||
                        (clientId != null && x.ShoppingCart.ClientId == clientId)
                    ));
            if (cartItem == null)
            {
                return false;
            }

            _dbContext.ShoppingCartItems.Remove(cartItem);

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _dbContext.ShoppingCartItems
                .Where(x => x.ShoppingCart.AppUserId == userId)
                .ToListAsync();
            _dbContext.ShoppingCartItems.RemoveRange(cart);
            await _dbContext.SaveChangesAsync();
        }
    }
}
