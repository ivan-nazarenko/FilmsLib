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
        [Required(ErrorMessage = "Поле обов'язкове")]
        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове")]
        [Display(Name = "Рік")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове")]
        [Display(Name = "Тривалість")]
        public int Duration { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове")]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове")]
        [Display(Name = "Посилання на трейлер")]
        [StringLength(int.MaxValue, MinimumLength = 20)]
        public string TrailerLink { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове, можливо ви не додали мови на сторінці Деталі?")]
        [Display(Name = "Мова")]
        public int LanguageId { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове, можливо ви не додали режисерів на сторінці Деталі?")]
        [Display(Name = "Режисер")]
        public int DirectorId { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове, можливо ви не додали жанри на сторінці Деталі?")]
        [Display(Name = "Жанри")]
        public List<int> Genres { get; set; }

        [Display(Name = "Фото:")]
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public byte[] PhotoFile { get; set; }
        public string ImageMimeType { get; set; }
    }
}
