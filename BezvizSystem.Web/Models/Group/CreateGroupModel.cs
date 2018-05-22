using BezvizSystem.Web.Models.Visitor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Group
{
    public class CreateGroupModel
    {
        public CreateGroupModel()
        {
            Infoes = new List<InfoVisitorModel>();
        }

        public int Id { get; set; }
        public bool Group { get; set; }

        [Display(Name = "Информация о туристах")]
        public ICollection<InfoVisitorModel> Infoes { get; set; }

        [Display(Name = "Дата прибытия")]
        [DataType(DataType.Date)]
        public DateTime? DateArrival { get; set; }
        [Display(Name = "Дата убывания")]
        [DataType(DataType.Date)]
        public DateTime? DateDeparture { get; set; }
        [Display(Name = "Количество дней пребывания")]
        public int? DaysOfStay { get; set; }
        [Display(Name = "Пункт пропуска для пребывания")]
        public string CheckPoint { get; set; }
        [Display(Name = "Инфо о месте пребывания")]
        public string PlaceOfRecidense { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Программа путешествия")]
        public string ProgramOfTravel { get; set; }
        [Display(Name = "Организационно-правовая форма")]
        public string OrganizeForm { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Номер договора с туристом")]
        public string NumberOfContract { get; set; }
        [Display(Name = "Дата заключения договора")]
        public DateTime? DateOfContract { get; set; }
        [Display(Name = "Другая информация")]
        public string OtherInfo { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
    }
}