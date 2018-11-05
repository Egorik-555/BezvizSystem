using BezvizSystem.Pogranec.Web.Infrastructure;
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
        [StringLength(99, MinimumLength = 6, ErrorMessage = "Длина УНП должна быть не менее 6 символов")]
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
        [RegularExpression(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$", ErrorMessage = "Некорректный IP-адрес")]
        public string Ip { get; set; }

        [Display(Name = "Уровень доступа")]
        public UserLevel Role { get; set; }

        public bool Active { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime? DateInSystem { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string UserInSystem { get; set; }

        public CreateUser()
        {
            Active = true;
            Role = UserLevel.GPKUser;
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
        [StringLength(99, MinimumLength = 6, ErrorMessage = "Длина УНП должна быть не менее 6 символов")]
        [DataType(DataType.Password)]
        public string ConfirmePassword { get; set; }

        [Display(Name = "ФИО сотрудника")]
        [Required(ErrorMessage = "Укажите ФИО сотрудника")]
        public string ProfileUserTranscript { get; set; }

        [Display(Name = "IP-адрес")]
        [Required(ErrorMessage = " Укажите ip-адрес")]
        [RegularExpression(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$", ErrorMessage = "Некорректный IP-адрес")]
        public string ProfileUserIp { get; set; }

        [Display(Name = "Уровень доступа")]
        [Required(ErrorMessage = " Укажите уровень доступа")]
        public UserLevel ProfileUserRole { get; set; }

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