using BookShop.Domain.Entities;
using BookShop.Utility.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.Interfaces
{
    public interface IBookRepository
    {
        Task<PageResult<Book>> GetAllAsync(string? search = null,
                                            string? category = null,
                                            string? author = null ,
                                            int pageNumber = 1, int pageSize = 8);
        Task<Book?> GetByIdAsync(Guid id);
        Task AddAsync(Book book);
        Task UpdateAsync();
        Task DeleteAsync(Guid id);
        Task<int> CountAsync();

        Task<IEnumerable<Book>> GetNewArrivalBooksAsync();
        Task<IEnumerable<Book>> GetSimilarBooksAsync(Guid bookId, Guid categoryId);
    }
}
