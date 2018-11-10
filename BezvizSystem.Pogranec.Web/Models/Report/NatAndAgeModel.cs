using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Pogranec.Web.Models.Report
{
    public class NatAndAgeModel
    {
        [Display(Name = "Страна")]
        public string Natiolaty { get; set; }
        [Display(Name = "Мужчины моложе 18")]
        public int? ManLess18 { get; set; }
        [Display(Name = "Мужчины старше 18")]
        public int? ManMore18 { get; set; }
        [Display(Name = "Женщины моложе 18")]
        public int? WomanLess18 { get; set; }
        [Display(Name = "Женщины старше 18")]
        public int? WomanMore18 { get; set; }
        [Display(Name = "Итого:")]
        public int? All { get; set; }
    }
}