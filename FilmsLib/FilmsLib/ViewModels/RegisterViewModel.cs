using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsLib.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введіть Email!")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введіть нікнейм!")]
        [Display(Name = "Нікнейм")]
        [StringLength(30, ErrorMessage = "Ім'я надто довге!")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Введіть вік!")]
        [Display(Name = "Вік")]
        [Range(16, int.MaxValue, ErrorMessage = "Зареєструватись можуть лише користувачі, які досягли 16 років")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Введіть пароль!")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Пароль надто короткий!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторіть пароль")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються!")]
        [DataType(DataType.Password)]
        [Display(Name = "Повторіть пароль")]
        public string PasswordConfirm { get; set; }
    }
}
