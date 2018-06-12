using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Логин не введён")]
        //[StringLength(50, MinimumLength = 4, ErrorMessage = "Длина строки должна быть более 6 символов")]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Парол не введён")]
        [DataType(DataType.Password)]
        //[StringLength(50, MinimumLength = 4, ErrorMessage = "Длина строки должна быть более 6 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}