using FilmsLib.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.ViewModels
{
    public class FilmViewModel
    {
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Ім'я")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Рік")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Тривалість")]
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Посилання на трейлер")]
        public string TrailerLink { get; set; }


        [Required]
        [Display(Name = "Мова")]
        public int LanguageId { get; set; }

        [Required]
        [Display(Name = "Режисер")]
        public int DirectorId { get; set; }

        [Required]
        [Display(Name = "Жанри")]
        public List<int> Genres { get; set; }

        [Display(Name = "Фото:")]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public byte[] PhotoFile { get; set; }
        public string ImageMimeType { get; set; }

        public FilmViewModel(Film film)
        {
            Id = film.Id;
            Name = film.Name;
            Year = film.Year;
            Duration = film.Duration;
            TrailerLink = film.TrailerLink;
            DirectorId = film.DirectorId;
            LanguageId = film.LanguageId;
        }
    }
}
