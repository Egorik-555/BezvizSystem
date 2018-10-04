using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Report
{
    public class CountByOperator
    {
        [Display(Name = "Туроператор")]
        public string Operator { get; set; }
        [Display(Name = "Количество зарегистрированных туристов")]
        public int? Count { get; set; }
    }
}