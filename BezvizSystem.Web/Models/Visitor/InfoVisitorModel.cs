using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Visitor
{
    public class InfoVisitorModel
    {
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Серия и номер паспорта")]
        public string SerialAndNumber { get; set; }
        [Display(Name = "Пол")]
        public string Gender { get; set; }
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime? BithDate { get; set; }
        [Display(Name = "Гражданство")]
        public string Nationality { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
    }
}