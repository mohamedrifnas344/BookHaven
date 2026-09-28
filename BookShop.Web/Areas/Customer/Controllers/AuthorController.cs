using BookShop.Application.Interfaces;
using BookShop.Application.ViewModels.Author;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if(author == null)
            {
                return NotFound();
            }

            var vm = new AuthorResponseVM
            {
                Id = author.Id,
                Name = author.Name,
                ImageUrl = author.ImageUrl,
                Biography = author.Biography
            };
            return View(vm);
        }
    }
}
