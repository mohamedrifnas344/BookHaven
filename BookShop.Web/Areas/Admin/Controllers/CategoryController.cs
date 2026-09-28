using BookShop.Application.Interfaces;
using BookShop.Application.ViewModels.Category;
using BookShop.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryVMs = categories.Select(category => new CategoryResponseVM
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();
            return View(categoryVMs);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddVM addVM)
        {
            if (!ModelState.IsValid)
            {
                return View(addVM);
            }

            var category = new Category
            {
                Name = addVM.Name
            };

            await _categoryRepository.AddAsync(category);
            TempData["success"] = "Category created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if(category == null)
            {
                return NotFound();
            }

            var vm = new CategoryEditVM
            {
                Id = category.Id,
                Name = category.Name
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditVM editVM)
        {
            if (!ModelState.IsValid)
            {
                return View(editVM);
            }

            var categoryFromDb = await _categoryRepository.GetByIdAsync(editVM.Id);
            if(categoryFromDb == null)
            {
                return NotFound();
            }

            categoryFromDb.Name = editVM.Name;
            await _categoryRepository.UpdateAsync();
            TempData["success"] = "Category updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            await _categoryRepository.DeleteAsync(id);
            TempData["success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
