using BookShop.Application.Interfaces;
using BookShop.Application.ViewModels.Rating;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class RatingController : Controller
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingController(
            IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Add Rating
        [HttpPost]
        public async Task<IActionResult> Add(RatingAddVM addVM)
        {
            if (!ModelState.IsValid)
            {
                return View(addVM);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }

            // Check whether user already rated this book
            var existingRating = await _ratingRepository
                .GetUserRatingAsync(addVM.BookId, userId);

            if (existingRating != null)
            {
                TempData["Error"] = "You have already rated this book.";
                return RedirectToAction("Details", "Book",
                    new { area = "Customer", id = addVM.BookId });
            }

            var rating = new Rating
            {
                BookId = addVM.BookId,
                AppUserId = userId,
                Value = addVM.Value,
                Review = addVM.Review,
                Status = RatingStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _ratingRepository.AddAsync(rating);

            TempData["Success"] =
                "Your rating has been submitted and is waiting for approval.";

            return RedirectToAction("Details", "Book",
                new { area = "Customer", id = addVM.BookId });
        }
    }
}