﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Pogranec.Web.Models.Report
{
    public class CountByOperatorModel
    {       
        public string Operator { get; set; }
        public int? Count { get; set; }
    }
}