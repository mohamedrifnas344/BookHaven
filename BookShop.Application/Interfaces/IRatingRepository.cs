using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.Interfaces
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAllAsync();
        Task<IEnumerable<Rating>> GetRatingsByBookIdAsync(Guid bookId);
        Task<Rating?> GetUserRatingAsync(Guid bookId, string userId);
        Task<Rating?> GetByIdAsync(Guid id);
        Task AddAsync(Rating rating);
        Task Update();
        Task Delete(Guid id);
    }
}
