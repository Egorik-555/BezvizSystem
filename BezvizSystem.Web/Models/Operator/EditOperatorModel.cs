using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Models.Operator
{
    public class EditOperatorModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string UserName { get; set; }
        [Required]
        [Display(Name ="Туроператор")]
        public string ProfileUserTranscript { get; set; }
        [Required]
        [Display(Name = "УНП")]
        public string ProfileUserUNP { get; set; }
        [Required]
        [Display(Name = "ОКПО")]
        public string ProfileUserOKPO { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "E-mail подтвержден")]
        public bool EmailConfirmed { get; set; }

        public string ProfileUserRole { get; set; }
        [Display(Name ="Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        public string ConfirmePassword { get; set; }
        [Display(Name = "Активный")]
        public bool ProfileUserActive { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime ProfileUserDateInSystem { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string ProfileUserUserInSystem { get; set; }

    }
}