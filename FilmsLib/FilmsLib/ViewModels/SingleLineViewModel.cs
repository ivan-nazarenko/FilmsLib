using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.ViewModels
{
    public class SingleLineViewModel
    {
        [Required(ErrorMessage = "Поле обов'язкове")]
        [Display(Name = "Назва")]
        public string Name { get; set; }
    }
}
