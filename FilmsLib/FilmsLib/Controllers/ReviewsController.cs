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
        public async Task<IActionResult> Index(string message = null, string error = null)
        {
            ViewBag.Error = error;
            ViewBag.Message = message;
            return View(await _reviewsRepository.GetReviewsAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Public(string error = null)
        {
            ViewBag.Error = error;
            return View(await _reviewsRepository.GetReviewsAsync());
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var reviewer = await _reviewerRepository.GetByUserId(user.Id);

            foreach(var rev in reviewer.Reviews)
            {
                if(rev.FilmId == id)
                {
                    return RedirectToAction("Profile", "Account", new { error = "Схоже ви вже залишали відгук на цей фільм, ви можете знайти його тут" });
                }
            }

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
                    ReviewerId = model.ReviewerId,
                    PublicatonDate = DateTime.Now
                };

                _reviewsRepository.Add(review);
                await _reviewsRepository.SaveChangesAsync();
                return RedirectToAction("Details", "Film", new { id = model.FilmId });
            }
            catch
            {
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var rev = await _reviewsRepository.GetByIdAsync(id);

            if (HttpContext.User.IsInRole("Admin"))
            {
                try
                {
                    _reviewsRepository.Delete(rev);
                    await _reviewsRepository.SaveChangesAsync();
                    return RedirectToAction("Index", "Reviews", new { message = "Відгук успішно видалено!" });
                }
                catch
                {
                    return RedirectToAction("Index", "Reviews", new { error = "Під час видалення відгуку сталася помилка!" });
                }
            }
            else
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (rev.Reviewer.UserId == user.Id)
                {
                    _reviewsRepository.Delete(rev);
                    await _reviewsRepository.SaveChangesAsync();
                    return RedirectToAction("Profile", "Account", new { message = "Відгук успішно видалено!" });
                }

                return RedirectToAction("Public", "Reviews", new { error = "Ви намагаєтесь видалити чужий відгук!" });
            }
        }

    }
}
