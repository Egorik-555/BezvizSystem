using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Pogranec.Web.Models.Report
{
    public class ViewTable1InExcel
    {
        public class ViewAnketaExcel
        {
            [Display(Name = "Страна")]
            public string Natiolaty { get; set; }
            [Display(Name = "Мужчин младше 18")]
            public string ManLess18 { get; set; }
            [Display(Name = "Мужчин старше 18")]
            public string ManMore18 { get; set; }
            [Display(Name = "Женщин младше 18")]
            public string WomanLess18 { get; set; }
            [Display(Name = "Женщин старше 18")]
            public string WomanMore18 { get; set; }
            [Display(Name = "Итого")]
            public string All { get; set; }          
        }
    }
}