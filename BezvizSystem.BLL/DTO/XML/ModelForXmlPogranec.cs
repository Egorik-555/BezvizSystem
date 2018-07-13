﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO.XML
{
    public class ModelForXmlPogranec
    {
        public int Id { get; set; }

        public int Organization { get; set; }
        public int TypeOperation { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public string DayBith { get; set; }
        public string MonthBith { get; set; }
        public string YearBith { get; set; }

        public string TextSex { get; set; }
        public int CodeSex { get; set; }

        public string DocNum { get; set; }
        public string DayValid { get; set; }
        public string MonthValid { get; set; }
        public string YearValid { get; set; }

        public int DayOfStay { get; set; }
        public string DayArrival { get; set; }
        public string MonthArrival { get; set; }
        public string YearArrival { get; set; }
    }
}
