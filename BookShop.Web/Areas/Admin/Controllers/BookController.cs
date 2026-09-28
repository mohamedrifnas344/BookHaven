using BookShop.Application.Interfaces;
using BookShop.Application.IServices;
using BookShop.Application.ViewModels.Book;
using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICloudinaryUploadService _uploadService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;

        public BookController(IBookRepository bookRepository , 
                              ICloudinaryUploadService uploadService ,
                              ICategoryRepository categoryRepository ,
                              IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository;
            _uploadService = uploadService;
            _categoryRepository = categoryRepository;
            _authorRepository = authorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search = null , string? category = null , 
                                               string? author = null , int pageNumber = 1, int pageSize = 8)
        {
            var result = await _bookRepository.GetAllAsync(search, category, author, pageNumber, pageSize);
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
                PublishedDate = book.PublishedDate
            }).ToList();
            return View(bookVMs);
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
                PublishedDate = book.PublishedDate
            };
            return View(bookVM);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var authors = await _authorRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookAddVM addVM)
        {
            var authors = await _authorRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.Authors = new SelectList(authors, "Id", "Name");
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(addVM);
            }

            string imageUrl;
            string publicId;
            try
            {
                var uploadResult = await _uploadService.UploadImageAsync(addVM.File);
                imageUrl = uploadResult.SecureUrl.ToString();
                publicId = uploadResult.PublicId;
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Image upload failed");
                ViewBag.Authors = new SelectList(authors, "Id", "Name");
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(addVM);
            }

            var book = new Book
            {
                Title = addVM.Title,
                Description = addVM.Description,
                Price = addVM.Price,
                SpecialPrice = addVM.SpecialPrice,
                Stock = addVM.Stock,
                PageNumber = addVM.PageNumber,
                ISBN = addVM.ISBN,
                ImageUrl = imageUrl,
                PublicId = publicId,
                IsFeatured = addVM.IsFeatured,
                IsNewArrival = addVM.IsNewArrival,
                PublishedDate = addVM.PublishedDate,
                CategoryId = addVM.CategoryId,
                AuthorId = addVM.AuthorId
            };

            await _bookRepository.AddAsync(book);
            TempData["success"] = "Book created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if(book == null)
            {
                return NotFound();
            }

            var authors = await _authorRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            var vm = new BookEditVM
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                SpecialPrice = book.SpecialPrice,
                Stock = book.Stock,
                PageNumber = book.PageNumber,
                ISBN = book.ISBN,
                ExistingImageUrl = book.ImageUrl,
                IsFeatured = book.IsFeatured,
                IsNewArrival = book.IsNewArrival,
                PublishedDate = book.PublishedDate,
                CategoryId = book.CategoryId,
                AuthorId = book.AuthorId
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookEditVM editVM)
        {
            var authors = await _authorRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                ViewBag.Authors = new SelectList(authors, "Id", "Name");
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(editVM);
            }

            var bookFromDb = await _bookRepository.GetByIdAsync(editVM.Id);
            if (bookFromDb == null)
            {
                return NotFound();
            }

            bookFromDb.Title = editVM.Title;
            bookFromDb.Description = editVM.Description;
            bookFromDb.Price = editVM.Price;
            bookFromDb.SpecialPrice = editVM.SpecialPrice;
            bookFromDb.Stock = editVM.Stock;
            bookFromDb.PageNumber = editVM.PageNumber;
            bookFromDb.IsFeatured = editVM.IsFeatured;
            bookFromDb.IsNewArrival = editVM.IsNewArrival;
            bookFromDb.ISBN = editVM.ISBN;
            bookFromDb.CategoryId = editVM.CategoryId;
            bookFromDb.AuthorId = editVM.AuthorId;
            bookFromDb.PublishedDate = editVM.PublishedDate;

            if (editVM.File != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(bookFromDb.PublicId))
                    {
                        await _uploadService.DeleteImageAsync(bookFromDb.PublicId);
                    }

                    var uploadResult = await _uploadService.UploadImageAsync(editVM.File);
                    bookFromDb.ImageUrl = uploadResult.SecureUrl.ToString();
                    bookFromDb.PublicId = uploadResult.PublicId;
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Image upload failed");
                    ViewBag.Authors = new SelectList(authors, "Id", "Name");
                    ViewBag.Categories = new SelectList(categories, "Id", "Name");
                    return View(editVM);
                }
            }
            await _bookRepository.UpdateAsync();
            TempData["success"] = "Book updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(book.PublicId))
            {
                await _uploadService.DeleteImageAsync(book.PublicId);
            }

            await _bookRepository.DeleteAsync(id);
            TempData["success"] = "Book deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdownsAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            ViewBag.Authors = new SelectList(authors, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }
    }
}
