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

        //[Required(ErrorMessage = "Информация о туристах не была введена")]
        [Display(Name = "Информация о туристах")]
        public ICollection<InfoVisitorModel> Infoes { get; set; }

        [Required(ErrorMessage = "Дата прибытия не введена")]
        [Display(Name = "Дата прибытия")]
        [Range(typeof(DateTime), "1/1/2000", "1/1/3000", ErrorMessage = "Неверный формат даты")]
        [DataType(DataType.Date)]
        public DateTime? DateArrival { get; set; }

        [Required(ErrorMessage = "Дата убывания не введена")]
        [Display(Name = "Дата убывания")]
        [Range(typeof(DateTime), "1/1/2000", "1/1/3000", ErrorMessage = "Неверный формат даты")]
        [DataType(DataType.Date)]
        public DateTime? DateDeparture { get; set; }

        //[Required(ErrorMessage = "Количество дней пребывания не была введено")]
        [RegularExpression(@"[0-9]{1,3}", ErrorMessage = "Только цифровые значения")]
        [Display(Name = "Количество дней пребывания")]
        public int? DaysOfStay { get; set; }

        [Required(ErrorMessage = "Пункт пропуска для пребывания не введен")]
        [Display(Name = "Пункт пропуска для пребывания")]
        public string CheckPoint { get; set; }

        [Display(Name = "Инфо о месте пребывания")]
        public string PlaceOfRecidense { get; set; }

       // [Required(ErrorMessage = "Программа путешествия не была введена")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Программа путешествия")]
        public string ProgramOfTravel { get; set; }

       // [Required(ErrorMessage = "Организационно-правовая форма не была введена")]
        [Display(Name = "Организационно-правовая форма")]
        public string OrganizeForm { get; set; }

       // [Required(ErrorMessage = "Название не было введено")]
        [Display(Name = "Название")]
        public string Name { get; set; }

       // [Required(ErrorMessage = "Номер договора с туристом не был введен")]
        [Display(Name = "Номер договора с туристом")]
        public string NumberOfContract { get; set; }

       // [Required(ErrorMessage = "Дата заключения договора не была введена")]
        [Display(Name = "Дата заключения договора")]
        public DateTime? DateOfContract { get; set; }

        [Display(Name = "Другая информация")]
        public string OtherInfo { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
    }
}