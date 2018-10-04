using BezvizSystem.BLL.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Report
{
    public class ReportModel
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? AllRegistrated { get; set; }
        public int? AllArrived { get; set; }
        public int? WaitArrived { get; set; }
        public int? NotArriverd { get; set; }
        public int? AllTourist { get; set; }
        public int? AllGroup { get; set; }

        public IEnumerable<NatAndAge> AllByNatAndAge { get; set; }
        public IEnumerable<CountByOperator> AllByOperatorCount { get; set; }

        public string StringDateByArrivalCount { get; set; }
        public string StringCheckPointCount { get; set; }
        public string StringDaysByCount { get; set; }
    }
}