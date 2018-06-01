using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Anketa
{
    public class ViewAnketaExcel
    {
        [Display(Name = "Группа")]
        public int Id { get; set; }
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }
        [Display(Name = "Имя")]
        public string Name { get; set; }
        [Display(Name = "Паспорт")]
        public string SerialAndNumber { get; set; }
        [Display(Name = "Гражданство")]
        public string Nationality { get; set; }
        [Display(Name = "Пол")]
        public string Gender { get; set; }
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime? BithDate { get; set; }
        [Display(Name = "Дата прибытия")]
        public DateTime? DateArrival { get; set; }
        [Display(Name = "Количество дней пребывания")]
        public int? DaysOfStay { get; set; }
        [Display(Name = "Пункт пропуска для въезда")]
        public string CheckPoint { get; set; }     
    }
}