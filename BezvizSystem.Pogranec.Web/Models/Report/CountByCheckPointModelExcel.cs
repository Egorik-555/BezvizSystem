using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Pogranec.Web.Models.Report
{
    public class CountByCheckPointModelExcel
    {
        [Display(Name = "Пункт пропуска")]
        public string CheckPoint { get; set; }
        [Display(Name = "Количество")]
        public string Count { get; set; }
    }

}