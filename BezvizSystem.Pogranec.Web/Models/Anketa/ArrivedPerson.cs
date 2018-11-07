using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Pogranec.Web.Models.Anketa
{

    public class ArrivedInfo
    {
        [Display(Name = "Пункт пропуска")]
        public string CheckPoint { get; set; }
        [Display(Name = "Количество прибывающих")]
        public int? Count { get; set; } 
    }

    public class ArrivedPerson
    {
        public int? Count { get; set; }
        public DateTime? ArriveFrom { get; set; }
        public DateTime? ArriveTo { get; set; }

        public IEnumerable<ArrivedInfo> Infoes { get; set; }

        public ArrivedPerson()
        {
            Infoes = new List<ArrivedInfo>();
        }
    }   
}