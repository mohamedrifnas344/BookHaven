using BookShop.Application.Interfaces;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _dbContext.Authors
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Authors
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Author author)
        {
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var author = await _dbContext.Authors
                .FirstOrDefaultAsync(x => x.Id == id);

            if (author == null)
                return;

            _dbContext.Authors.Remove(author);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Authors.CountAsync();
        }
    }
}
