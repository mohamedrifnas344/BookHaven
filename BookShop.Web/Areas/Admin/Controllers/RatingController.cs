using BookShop.Application.Interfaces;
using BookShop.Application.ViewModels.Rating;
using BookShop.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    public class RatingController : Controller
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingController(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ratings = await _ratingRepository.GetAllAsync();
            var ratingVMs = ratings.Select(rating => new RatingResponseVM
            {
                Id = rating.Id,
                Value = rating.Value,
                Review = rating.Review,
                CreatedAt = rating.CreatedAt,
                Status = rating.Status,
                BookId = rating.Book.Id,
                BookTitle = rating.Book.Title,
                AppUserId = rating.AppUserId,
                FirstName = rating.AppUser.FirstName,
                LastName = rating.AppUser.LastName
            }).ToList();

            return View(ratingVMs);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid id, RatingStatus status)
        {
            var rating = await _ratingRepository.GetByIdAsync(id);
            if (rating == null)
            {
                return NotFound();
            }

            rating.Status = status;

            await _ratingRepository.Update();
            TempData["success"] = "Review status updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
