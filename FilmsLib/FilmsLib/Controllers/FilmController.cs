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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FilmsLib.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FilmController : Controller
    {
        private readonly ILogger<FilmController> _logger;
        private readonly IDetailsRepository _detailsRepository;
        private readonly IFilmRepository _filmRepository;
        private readonly IWebHostEnvironment _environment;

        public FilmController(ILogger<FilmController> logger, IDetailsRepository detailsRepository, IWebHostEnvironment environment, IFilmRepository filmRepository)
        {
            _logger = logger;
            _detailsRepository = detailsRepository;
            _environment = environment;
            _filmRepository = filmRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View(await _filmRepository.GetFilmsAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewData["Languages"] = await _detailsRepository.GetLanguagesAsync();
            ViewData["Genres"] = await _detailsRepository.GetGenresAsync();
            ViewData["Directors"] = await _detailsRepository.GetDirectorsAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FilmViewModel model)
        {
            if(!ModelState.IsValid)
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
                    TrailerLink = model.TrailerLink,
                    LanguageId = model.LanguageId,
                    DirectorId = model.DirectorId
                };

                _filmRepository.Add(film);

                if(await _filmRepository.SaveChangesAsync())
                {
                    _filmRepository.AddGenres(film.Id, model.Genres);

                    var cover = new Cover
                    {
                        FilmId = film.Id
                    };

                    _filmRepository.AddCover(cover, model.Image);

                    await _filmRepository.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FilmController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FilmController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var film = await _filmRepository.GetByIdAsync(id);
                _filmRepository.Delete(film);
                await _filmRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
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
