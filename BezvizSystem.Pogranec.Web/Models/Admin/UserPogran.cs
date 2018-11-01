using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Models.Admin
{
    public class DisplayUser
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "Логин")]
        public string UserName { get; set; }
        [Display(Name = "ФИО сотрудника")]
        public string ProfileUserTranscript { get; set; }
        [Display(Name = "Ip-адрес")]
        public string ProfileUserIp { get; set; }
    }


    public class CreateUser
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Укажите логин")]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [Display(Name = "Подтверждение пароля")]
        [Required(ErrorMessage = "Укажите подтверждение пароля")]
        [DataType(DataType.Password)]
        public string ConfirmePassword { get; set; }

        [Display(Name = "ФИО сотрудника")]
        [Required(ErrorMessage = "Укажите ФИО сотрудника")]
        public string Transcript { get; set; }

        [Display(Name = "IP-адрес")]
        [Required(ErrorMessage = " Укажите ip-адрес")]
        public string Ip { get; set; }

        public string Role { get; set; }
        public bool Active { get; set; }
        //public DateTime? NotActiveToDate { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? DateInSystem { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string UserInSystem { get; set; }

        public CreateUser()
        {
            Active = true;
            Role = "pogranec";
        }
    }

    public class EditUser
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "Логин - не редактируется")]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        public string ConfirmePassword { get; set; }

        [Display(Name = "ФИО сотрудника")]
        [Required(ErrorMessage = "Укажите ФИО сотрудника")]
        public string ProfileUserTranscript { get; set; }

        [Display(Name = "IP-адрес")]
        [Required(ErrorMessage = " Укажите ip-адрес")]
        public string ProfileUserIp { get; set; }

        [Display(Name = "Активен")]
        public bool ProfileUserActive { get; set; }
        [Display(Name = "До какого момента не активен")]
        public DateTime? ProfileUserNotActiveToDate { get; set; }


        [HiddenInput(DisplayValue = false)]
        public DateTime ProfileUserDateInSystem { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string ProfileUserUserInSystem { get; set; }
        [HiddenInput(DisplayValue = false)]
        public DateTime ProfileUserDateEdit { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string ProfileUserUserEdit { get; set; }
    }

    public class DeleteUser
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "Логин")]
        public string UserName { get; set; }
        [Display(Name = "ФИО сотрудника")]
        public string ProfileUserTranscript { get; set; }
    }
}