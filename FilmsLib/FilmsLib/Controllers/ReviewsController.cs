using FilmsLib.Models;
using FilmsLib.Services.Interfaces;
using FilmsLib.Services.Repositories;
using FilmsLib.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewsRepository _reviewsRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly UserManager<User> _userManager;

        public ReviewsController(IReviewsRepository reviewsRepository, IReviewerRepository reviewerRepository, UserManager<User> userManager)
        {
            _reviewsRepository = reviewsRepository;
            _reviewerRepository = reviewerRepository;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _reviewsRepository.GetReviewsAsync());
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var reviewer = await _reviewerRepository.GetByUserId(user.Id);

            var model = new ReviewViewModel
            {
                FilmId = id,
                ReviewerId = reviewer.Id
            };

            return View(model);

        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Create(ReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var review = new Review
                {
                    Mark = model.Mark,
                    Heading = model.Heading,
                    Text = model.Text,
                    FilmId = model.FilmId,
                    ReviewerId = model.ReviewerId
                };

                _reviewsRepository.Add(review);
                await _reviewsRepository.SaveChangesAsync();
                return RedirectToAction("Index", "Film");
            }
            catch
            {
                return View(model);
            }
        }

    }
}
