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
        [Display(Name="Логин")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пароль не введён")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
     
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}