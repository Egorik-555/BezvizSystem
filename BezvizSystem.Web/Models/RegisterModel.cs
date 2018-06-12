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
        //[StringLength(MaximumLength = 10, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 10 символов")]
        [RegularExpression(@"\d", ErrorMessage = "Допустимы только цифровые символы")]
        [Display(Name = "ОКПО")]
        public string OKPO { get; set; }

        [Required(ErrorMessage = "УНП не введён")]
       // [StringLength(MaximumLength = 9, MinimumLength = 9, ErrorMessage = "Длина строки должна быть 9 символов")]
        [RegularExpression(@"[0-9]{3,10}", ErrorMessage = "Допустимы только цифровые символы")]
        [Display(Name = "УНП")]
        public string UNP { get; set; }

        [Required(ErrorMessage = "Email не введён")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}