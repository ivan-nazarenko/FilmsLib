using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FilmsLib.Models;
using FilmsLib.Services.Interfaces;
using FilmsLib.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FilmsLib.Controllers
{
    [Authorize]
    public class FilmController : Controller
    {
        private readonly ILogger<FilmController> _logger;
        private readonly IDetailsRepository _detailsRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IReviewsRepository _reviewsRepository;
        private readonly IReviewerRepository _reviewerRepository;
        private readonly UserManager<User> _userManager;

        public FilmController(ILogger<FilmController> logger,
                              IDetailsRepository detailsRepository,
                              IWebHostEnvironment environment,
                              IFilmRepository filmRepository,
                              IReviewsRepository reviewsRepository,
                              IReviewerRepository reviewerRepository,
                              UserManager<User> userManager)
        {
            _logger = logger;
            _detailsRepository = detailsRepository;
            _environment = environment;
            _filmRepository = filmRepository;
            _reviewerRepository = reviewerRepository;
            _reviewsRepository = reviewsRepository;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(await _filmRepository.GetFilmsAsync());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string query)
        {
            if (query == null)
            {
                return RedirectToAction("Index", "Film");
            }

            ViewBag.Search = query;
            return View(await _filmRepository.GetByName(query));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Sorted(string type, int id)
        {
            try
            {
                if (type == "dir")
                {
                    var dir = await _detailsRepository.GetDirectorByIdAsync(id);
                    ViewBag.Sort = $"{dir.FirstName} {dir.LastName}";
                    var films = await _filmRepository.GetByDirectorAsync(id);
                    return View(films);
                }
                else
                {
                    var genre = await _detailsRepository.GetGenreByIdAsync(id);
                    ViewBag.Sort = genre.Name;
                    var films = await _detailsRepository.GetFilmsByGenre(id);
                    return View(films);
                }
            }
            catch
            {
                return RedirectToAction("Index", "Film");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id, string message = null)
        {
            ViewBag.Message = message;
            ViewData["Mark"] = await _reviewsRepository.GetAverageMarkAsync(id);
            ViewBag.Reviews = await _reviewsRepository.GetByFilmIdAsync(id);
            var film = await _filmRepository.GetByIdAsync(id);
            return View(film);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewData["Languages"] = await _detailsRepository.GetLanguagesAsync();
            ViewData["Genres"] = await _detailsRepository.GetGenresAsync();
            ViewData["Directors"] = await _detailsRepository.GetDirectorsAsync();

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FilmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var film = new Film
                {
                    Name = model.Name,
                    Year = model.Year,
                    Duration = model.Duration,
                    Description = model.Description,
                    TrailerLink = model.TrailerLink[^11..],
                    LanguageId = model.LanguageId,
                    DirectorId = model.DirectorId
                };

                _filmRepository.Add(film);

                if (await _filmRepository.SaveChangesAsync())
                {
                    _filmRepository.AddGenres(film.Id, model.Genres);

                    var cover = new Cover
                    {
                        FilmId = film.Id
                    };

                    _filmRepository.AddCover(cover, model.Image);

                    await _filmRepository.SaveChangesAsync();
                }

                return RedirectToAction("FilmsTable", "Film", new { message = "Фільм успішно додано!" });
            }
            catch
            {
                return RedirectToAction("Create", "Film");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var film = await _filmRepository.GetByIdAsync(id);
            var model = new FilmEditViewModel(film);

            ViewData["Languages"] = await _detailsRepository.GetLanguagesAsync();
            ViewData["Genres"] = await _detailsRepository.GetGenresAsync();
            ViewData["Directors"] = await _detailsRepository.GetDirectorsAsync();

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> Edit(FilmEditViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var film = await _filmRepository.GetByIdAsync((int)model.Id);

                film.Id = model.Id;
                film.Name = model.Name;
                film.Year = model.Year;
                film.Duration = model.Duration;
                film.TrailerLink = model.TrailerLink[^11..];
                film.Description = model.Description;
                film.DirectorId = model.DirectorId;
                film.LanguageId = model.LanguageId;

                await _filmRepository.SaveChangesAsync();

                return RedirectToAction("FilmsTable", "Film", new { message = "Фільм успішно відредаговано!" });
            }
            catch
            {
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var film = await _filmRepository.GetByIdAsync(id);
                _filmRepository.Delete(film);
                await _filmRepository.SaveChangesAsync();
                return RedirectToAction("FilmsTable", "Film", new { message = "Фільм успішно видалено!" });
            }
            catch
            {
                return RedirectToAction("FilmsTable", "Film", new { error = "Під час видалення фільму сталася помилка:(" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> FilmsTable(string message = null, string error = null)
        {
            ViewBag.Message = message;
            ViewBag.Error = error;
            return View(await _filmRepository.GetFilmsAsync());
        }


        [AllowAnonymous]
        public async Task<IActionResult> GetImage(int id)
        {
            var requestedFilm = await _filmRepository.GetByIdAsync(id);
            if (requestedFilm != null)
            {
                string webRootpath = _environment.WebRootPath;
                string folderPath = "\\images\\";
                string fullPath = webRootpath + folderPath + requestedFilm.Cover.ImageName;
                if (System.IO.File.Exists(fullPath))
                {
                    FileStream fileOnDisk = new FileStream(fullPath, FileMode.Open);
                    byte[] fileBytes;
                    using (BinaryReader br = new BinaryReader(fileOnDisk))
                    {
                        fileBytes = br.ReadBytes((int)fileOnDisk.Length);
                    }
                    return File(fileBytes, requestedFilm.Cover.ImageMimeType);
                }
                else
                {
                    if (requestedFilm.Cover.PhotoFile.Length > 0)
                    {
                        return File(requestedFilm.Cover.PhotoFile, requestedFilm.Cover.ImageMimeType);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}
