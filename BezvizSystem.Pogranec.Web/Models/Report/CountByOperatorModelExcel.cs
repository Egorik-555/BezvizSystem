﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Pogranec.Web.Models.Report
{
    public class CountByOperatorModelExcel
    {
        [Display(Name = "Туроператор")]
        public string Operator { get; set; }
        [Display(Name = "Количество")]
        public string Count { get; set; }
    }
}