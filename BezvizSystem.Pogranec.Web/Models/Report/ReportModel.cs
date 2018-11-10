using BezvizSystem.BLL.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BezvizSystem.Pogranec.Web.Models.Report
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

        public IEnumerable<NatAndAgeModel> AllByNatAndAge { get; set; }
        public IEnumerable<CountByOperatorModel> AllByOperatorCount { get; set; }

        public IEnumerable<CountByDateModel> AllByDateArrivalCount { get; set; }
        public IEnumerable<CountByCheckPointModel> AllByCheckPointCount { get; set; }
        public IEnumerable<CountByDaysModel> AllByDaysCount { get; set; }

        public string StringDateByArrivalCount { get; set; }
        public string StringCheckPointCount { get; set; }
        public string StringDaysByCount { get; set; }
    }
}