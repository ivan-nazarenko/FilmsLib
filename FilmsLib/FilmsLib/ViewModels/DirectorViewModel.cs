using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.ViewModels
{
    public class DirectorViewModel
    {
        [Required(ErrorMessage = "Поле обов'язкове")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле обов'язкове")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }
    }
}
