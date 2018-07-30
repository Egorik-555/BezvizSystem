using BezvizSystem.BLL.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Report.DTO
{
    public class ReportDTO
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? AllRegistrated { get; set; }
        public int? AllArrived { get; set; }
        public int? WaitArrived { get; set; }
        public int? NotArriverd { get; set; }
        public int? AllTourist { get; set; }
        public int? AllGroup { get; set; }
        public int? AllTouristInGroup { get; set; }

        public IEnumerable<NatAndAge> AllByNatAndAge { get; set; }
        public IEnumerable<CountByDate> AllByDateArrivalCount { get; set; }
        public IEnumerable<CountByCheckPoint> AllByCheckPointCount { get; set; }
        public IEnumerable<CountByDays> AllByDaysCount { get; set; }
        public IEnumerable<CountByOperator> AllByOperatorCount { get; set; }
    }
}
