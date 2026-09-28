using BookShop.Application.Error;
using BookShop.Application.Interfaces;
using BookShop.Application.ViewModels.Author;
using BookShop.Application.ViewModels.Book;
using BookShop.Application.ViewModels.Common;
using BookShop.Application.ViewModels.Rating;
using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public HomeController(IBookRepository bookRepository ,
                              IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var newArrivalBooks = await _bookRepository.GetNewArrivalBooksAsync();
            var newArrivalBookVMs = newArrivalBooks.Select(book => new BookResponseVM
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

            var homeVM = new HomePageResponseVM
            {
                NewArrivalBooks = newArrivalBookVMs,
                Authors = authorVMs
            };

            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
