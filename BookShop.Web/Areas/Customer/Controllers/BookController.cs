using BookShop.Application.Interfaces;
using BookShop.Application.ViewModels.Author;
using BookShop.Application.ViewModels.Book;
using BookShop.Application.ViewModels.Category;
using BookShop.Application.ViewModels.Common;
using BookShop.Application.ViewModels.Rating;
using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRatingRepository _ratingRepository;

        public BookController(IBookRepository bookRepository ,
                              IAuthorRepository authorRepository ,
                              ICategoryRepository categoryRepository ,
                              IRatingRepository ratingRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _ratingRepository = ratingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search = null , 
                                               string? category = null , 
                                               string? author = null ,
                                               int pageNumber = 1 , int pageSize = 8)
        {
            var result = await _bookRepository.GetAllAsync(search , category , author , pageNumber , pageSize);
            var totalCount = result.TotalCount;
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.Author = author;
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = pageNumber > 1;
            ViewBag.HasNextPage = pageNumber < totalPages;

            var bookVMs = result.Items.Select(book => new BookResponseVM
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                SpecialPrice = book.SpecialPrice,
                Stock = book.Stock,
                PageNumber = book.PageNumber,
                ISBN = book.ISBN,
                ImageUrl = book.ImageUrl,
                IsFeatured = book.IsFeatured,
                IsNew = book.IsNewArrival,
                CategoryName = book.Category.Name,
                AuthorName = book.Author.Name,
                PublishedDate = book.PublishedDate,

                RatingAverageCounts = book.Ratings.Select(rate => new RatingAverageCountVM
                {
                    Value = rate.Value,
                    Status = rate.Status
                }).ToList()
            }).ToList();

            var authors = await _authorRepository.GetAllAsync();
            var authorVMs = authors.Select(author => new AuthorResponseVM
            {
                Id = author.Id,
                Name = author.Name,
                ImageUrl = author.ImageUrl,
                Biography = author.Biography
            }).ToList();

            var categories = await _categoryRepository.GetAllAsync();
            var categoryVMs = categories.Select(category => new CategoryResponseVM
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();

            var bookPage = new BookPageResponseVM
            {
                Books = bookVMs,
                Authors = authorVMs,
                Categories = categoryVMs,
            };
            return View(bookPage);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if(book == null)
            {
                return NotFound();
            }

            var bookVM = new BookResponseVM
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                SpecialPrice = book.SpecialPrice,
                Stock = book.Stock,
                PageNumber = book.PageNumber,
                ISBN = book.ISBN,
                ImageUrl = book.ImageUrl,
                IsFeatured = book.IsFeatured,
                IsNew = book.IsNewArrival,
                CategoryName = book.Category.Name,
                AuthorName = book.Author.Name,
                PublishedDate = book.PublishedDate ,

                RatingResponse = book.Ratings.Select(rating => new RatingResponseVM
                {
                    Id = rating.Id,
                    Value = rating.Value,
                    Review = rating.Review,
                    CreatedAt = rating.CreatedAt,
                    Status = rating.Status,
                    BookTitle = rating.Book.Title,
                    AppUserId = rating.AppUserId,
                    FirstName = rating.AppUser.FirstName,
                    LastName = rating.AppUser.LastName
                }).ToList(),
            };

            var similarBooks = await _bookRepository.GetSimilarBooksAsync(book.Id ,book.Category.Id);
            var similarBookVM = similarBooks.Select(book => new BookResponseVM
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                SpecialPrice = book.SpecialPrice,
                Stock = book.Stock,
                PageNumber = book.PageNumber,
                ISBN = book.ISBN,
                ImageUrl = book.ImageUrl,
                IsFeatured = book.IsFeatured,
                IsNew = book.IsNewArrival,
                CategoryName = book.Category.Name,
                AuthorName = book.Author.Name,
                PublishedDate = book.PublishedDate,

                RatingAverageCounts = book.Ratings.Select(rate => new RatingAverageCountVM
                {
                    Value = rate.Value,
                    Status = rate.Status
                }).ToList()
            }).ToList();

            var bookDetailsVM = new BookDetailsResponseVM
            {
                Book = bookVM,
                SimilarBooks = similarBookVM,
                CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                RatingAverageCounts = book.Ratings.Select(rate => new RatingAverageCountVM
                {
                    Value = rate.Value,
                    Status = rate.Status
                }).ToList()
            };
            return View(bookDetailsVM);
        }
    }
}
