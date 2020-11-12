using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.ViewModels
{
    public class ReviewViewModel
    {
        [Required]
        [Display(Name = "Оцінка")]
        public int Mark { get; set; }

        [Required]
        [Display(Name = "Заголовок")]
        [StringLength(30, ErrorMessage = "Заголовок надто довгий")]
        public string Heading { get; set; }

        [Required]
        [Display(Name = "Текст")]
        public string Text { get; set; }

        public string FilmName { get; set; }

        public int FilmId { get; set; }
        public int ReviewerId { get; set; }
    }
}
