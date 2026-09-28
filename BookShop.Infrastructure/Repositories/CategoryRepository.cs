using BookShop.Application.Interfaces;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return;

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Categories.CountAsync();
        }
    }
}
