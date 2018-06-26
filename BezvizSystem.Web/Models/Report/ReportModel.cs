using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Report
{
    public class ReportModel
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string AllRegistrated { get; set; }
        public string AllArrived { get; set; }
        public string WaitArrived { get; set; }
        public string NotArriverd { get; set; }
        public string AllTourist { get; set; }
        public string AllGroup { get; set; }
    }
}