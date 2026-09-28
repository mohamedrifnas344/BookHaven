using BookShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Application.Interfaces
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(Guid id);
        Task AddAsync(Author author);
        Task UpdateAsync();
        Task DeleteAsync(Guid id);
        Task<int> CountAsync();
    }
}
