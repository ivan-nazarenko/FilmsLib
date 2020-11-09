using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmsLib.Models;
using FilmsLib.Services.Interfaces;
using FilmsLib.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FilmsLib.Controllers
{
    public class DetailsController : Controller
    {
        private readonly ILogger<DetailsController> _logger;
        private readonly IDetailsRepository _detailsRepository;

        public DetailsController(ILogger<DetailsController> logger, IDetailsRepository detailsRepository)
        {
            _logger = logger;
            _detailsRepository = detailsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new DetailsViewModel
            {
                Directors = await _detailsRepository.GetDirectorsAsync(),
                Languages = await _detailsRepository.GetLanguagesAsync(),
                Genres = await _detailsRepository.GetGenresAsync()
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult AddGenre()
        {
            return PartialView("_GenreModalPartial", new SingleLineViewModel { });
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre(SingleLineViewModel model)
        {
            if(ModelState.IsValid)
            {
                var genre = new Genre
                {
                    Name = model.Name
                };

                _detailsRepository.AddGenre(genre);

                await _detailsRepository.SaveChangesAsync();

                return PartialView("_GenreModalPartial", model);
            }

            return PartialView("_GenreModalPartial", model);
        }

        [Route("DeleteGenre/{id:int}")]
        [HttpPost]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _detailsRepository.GetGenreByIdAsync(id);
            _detailsRepository.DeleteGenre(genre);
            await _detailsRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        public IActionResult AddLanguage()
        {
            return PartialView("_LangModalPartial", new SingleLineViewModel { });
        }

        [HttpPost]
        public async Task<IActionResult> AddLanguage(SingleLineViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lang = new Language
                {
                    Name = model.Name
                };

                _detailsRepository.AddLanguage(lang);

                await _detailsRepository.SaveChangesAsync();

                return PartialView("_LangModalPartial", model);
            }

            return PartialView("_LangModalPartial", model);
        }

        [Route("DeleteLanguage/{id:int}")]
        [HttpPost]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            var lang = await _detailsRepository.GetLanguageByIdAsync(id);
            _detailsRepository.DeleteLanguage(lang);
            await _detailsRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet]
        public IActionResult AddDirector()
        {
            return PartialView("_DirectorModalPartial", new DirectorViewModel { });
        }

        [HttpPost]
        public async Task<IActionResult> AddDirector(DirectorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dir = new Director
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                _detailsRepository.AddDirector(dir);

                await _detailsRepository.SaveChangesAsync();

                return PartialView("_DirectorModalPartial", model);
            }

            return PartialView("_DirectorModalPartial", model);
        }

        [Route("DeleteDirector/{id:int}")]
        [HttpPost]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            var dir = await _detailsRepository.GetDirectorByIdAsync(id);
            _detailsRepository.DeleteDirector(dir);
            await _detailsRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
