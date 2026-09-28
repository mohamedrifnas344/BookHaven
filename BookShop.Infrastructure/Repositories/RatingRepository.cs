using BookShop.Application.Interfaces;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RatingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Rating rating)
        {
            await _dbContext.Ratings.AddAsync(rating);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var rating = await _dbContext.Ratings.FindAsync(id);
            if (rating == null) return;
            _dbContext.Ratings.Remove(rating);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Rating>> GetAllAsync()
        {
            return await _dbContext.Ratings
                .Include(r => r.Book)
                .Include(r => r.AppUser)
                .ToListAsync();
        }

        public async Task<Rating?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Ratings
            .Include(r => r.AppUser)
            .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rating>> GetRatingsByBookIdAsync(Guid bookId)
        {
            return await _dbContext.Ratings
                 .Where(r => r.BookId == bookId &&
                 r.Status == RatingStatus.Approved)
                .Include(r => r.AppUser)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Rating?> GetUserRatingAsync(Guid bookId, string userId)
        {
            return await _dbContext.Ratings
            .FirstOrDefaultAsync(r =>
                r.BookId == bookId &&
                r.AppUserId == userId);
        }

        public async Task Update()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
