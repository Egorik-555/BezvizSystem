using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "ОКПО не введён")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "Допустимы только цифровые символы")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 10 символов")]      
        [Display(Name = "ОКПО")]
        public string OKPO { get; set; }

        [Required(ErrorMessage = "УНП не введён")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Длина строки должна быть 9 символов")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "Допустимы только цифровые символы")]
        [Display(Name = "УНП")]
        public string UNP { get; set; }

        [Required(ErrorMessage = "Email не введён")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}