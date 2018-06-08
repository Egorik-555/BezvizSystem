using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "ОКПО не был введён")]
        [RegularExpression(@"\d{3,10}", ErrorMessage = "Только цифровые значения")]
        [Display(Name = "ОКПО")]
        public string OKPO { get; set; }

        [Required(ErrorMessage = "УНП не был введён")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Длина строки должна быть 9 символов")]
        [RegularExpression(@"[0-9]{3,10}", ErrorMessage = "Только цифровые значения")]
        [Display(Name = "УНП")]
        public string UNP { get; set; }

        [Required(ErrorMessage = "Email не был введён")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Некорректный адрес")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}