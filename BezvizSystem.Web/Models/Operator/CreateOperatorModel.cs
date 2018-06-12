using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Models.Operator
{
    public class CreateOperatorModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Наименование не введено")]
        [Display(Name = "Наименование")]
        public string Transcript { get; set; }

        [Required(ErrorMessage = "УНП не введён")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Длина строки должна быть 9 символов")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "Допустимы только цифровые символы")]
        [Display(Name = "УНП")]
        public string UNP { get; set; }

        [Required(ErrorMessage = "ОКПО не введён")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 10 символов")]
        [RegularExpression(@"[0-9]*", ErrorMessage = "Допустимы только цифровые символы")]
        [Display(Name = "ОКПО")]
        public string OKPO { get; set; }

        public string Role { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime DateInSystem { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string UserInSystem { get; set; }

        public CreateOperatorModel()
        {
            Active = true;
            Role = "operator";
            Password = "111222";
        }
    }
}