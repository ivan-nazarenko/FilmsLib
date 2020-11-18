using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmsLib.Models;
using FilmsLib.Services.Interfaces;
using FilmsLib.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FilmsLib.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IReviewsRepository _reviewsRepository;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IReviewerRepository reviewerRepository, IReviewsRepository reviewsRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _reviewerRepository = reviewerRepository;
            _reviewsRepository = reviewsRepository;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var test = await _reviewerRepository.GetByNicknameAsync(model.Nickname);
                if (test == null)
                {
                    User user = new User
                    {
                        Email = model.Email,
                        UserName = model.Email
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "User");

                        Reviewer reviewer = new Reviewer
                        {
                            UserId = user.Id,
                            Nickname = model.Nickname,
                            Age = model.Age
                        };

                        _reviewerRepository.Add(reviewer);
                        await _reviewerRepository.SaveChangesAsync();

                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

                ModelState.AddModelError("", "Цей нікнейм вже зайнято!");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Невірний логін або пароль");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string message = null)
        {
            ViewBag.Message = message;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var reviewer = await _reviewerRepository.GetByUserId(user.Id);
            ViewBag.Reviews = await _reviewsRepository.GetByReviewerIdAsync(reviewer.Id);
            return View(reviewer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
