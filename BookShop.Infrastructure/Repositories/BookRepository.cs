using BookShop.Application.Interfaces;
using BookShop.Domain.Entities;
using BookShop.Infrastructure.Data;
using BookShop.Utility.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PageResult<Book>> GetAllAsync(string? search = null,
                                                         string? category = null,
                                                         string? author = null ,
                                                         int pageNumber = 1, int pageSize = 8)
        {
            var query = _dbContext.Books
                .Include(q => q.Category)
                .Include(q => q.Author)
                .Include(r => r.Ratings)
                .AsNoTracking()
                .AsQueryable();

            //Search
            // Search title, category and author
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(b =>
                    b.Title.Contains(search) ||
                    b.Category.Name.Contains(search) ||
                    b.Author.Name.Contains(search));
            }

            //Filtering by category
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(c => c.Category.Name.ToLower() == category.ToLower());
            }

            //Filtering by author
            if (!string.IsNullOrWhiteSpace(author))
            {
                query = query.Where(a => a.Author.Name.ToLower() == author.ToLower());
            }

            //Pagination
            var skipResult = (pageNumber - 1) * pageSize;

            var totalCount = await query.CountAsync();
            var items = await query.Skip(skipResult).Take(pageSize).ToListAsync();

            var pageResult = new PageResult<Book>()
            {
                Items = items,
                TotalCount = totalCount
            };

            return pageResult; ;
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Books
                .Include(q => q.Category)
                .Include(q => q.Author)
                .Include(r => r.Ratings)
                .ThenInclude(r => r.AppUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Book book)
        {
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _dbContext.Books
                .FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
                return;

            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Books.CountAsync();
        }

        public async Task<IEnumerable<Book>> GetNewArrivalBooksAsync()
        {
            return await _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(r => r.Ratings)
                .Where(b => b.IsNewArrival)
                .OrderByDescending(b => b.PublishedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetSimilarBooksAsync(
                 Guid bookId,
                 Guid categoryId)
        {
            return await _dbContext.Books
                .Where(b => b.CategoryId == categoryId && b.Id != bookId)
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(r => r.Ratings)
                .ToListAsync();
        }
    }
}
