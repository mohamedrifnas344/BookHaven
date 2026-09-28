using BookShop.Application.Interfaces;
using BookShop.Application.IServices;
using BookShop.Application.ViewModels.Author;
using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class AuthorController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ICloudinaryUploadService _uploadService;

        public AuthorController(IAuthorRepository authorRepository , 
                                ICloudinaryUploadService uploadService)
        {
            _authorRepository = authorRepository;
            _uploadService = uploadService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var authors = await _authorRepository.GetAllAsync();
            var authorsVMs = authors.Select(author => new AuthorResponseVM
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                ImageUrl = author.ImageUrl
            }).ToList();
            return View(authorsVMs);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AuthorAddVM addVM)
        {
            if (!ModelState.IsValid)
            {
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
                return View(addVM);
            }

            //Map to Domain
            var author = new Author
            {
                Name = addVM.Name,
                ImageUrl = imageUrl,
                Biography = addVM.Biography,
                PublicId = publicId
            };

            await _authorRepository.AddAsync(author);
            TempData["success"] = "Author created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if(author == null)
            {
                return NotFound();
            }

            var vm = new AuthorEditVM
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                ExistingImageUrl = author.ImageUrl
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AuthorEditVM editVM)
        {
            if (!ModelState.IsValid)
            {
                return View(editVM);
            }

            var authorFromDb = await _authorRepository.GetByIdAsync(editVM.Id);
            if(authorFromDb == null)
            {
                return NotFound();
            }

            authorFromDb.Name = editVM.Name;
            authorFromDb.Biography = editVM.Biography;

            if (editVM.File != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(authorFromDb.PublicId))
                    {
                        await _uploadService.DeleteImageAsync(authorFromDb.PublicId);
                    }

                    var uploadResult = await _uploadService.UploadImageAsync(editVM.File);
                    authorFromDb.ImageUrl = uploadResult.SecureUrl.ToString();
                    authorFromDb.PublicId = uploadResult.PublicId;
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Image upload failed");
                    return View(editVM);
                }
            }
            await _authorRepository.UpdateAsync();
            TempData["success"] = "Author updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(author.PublicId))
            {
                await _uploadService.DeleteImageAsync(author.PublicId);
            }
            await _authorRepository.DeleteAsync(id);
            TempData["success"] = "Author deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
