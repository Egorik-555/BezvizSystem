using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Report;
using BezvizSystem.BLL.Report.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IReport : IDisposable
    {
        ReportDTO GetReport();
        ReportDTO GetReport(string checkPoint);
        ReportDTO GetReport(DateTime? dateMoment);
        ReportDTO GetReport(DateTime? dateFrom, DateTime? dateTo);
        ReportDTO GetReport(DateTime? dateFrom, DateTime? dateTo, string checkPoint);
        ReportDTO GetReport(DateTime? dateFrom, DateTime? dateTo, DateTime? dateMoment);
        ReportDTO GetReport(DateTime? dateFrom, DateTime? dateTo, string checkPoint, DateTime? dateMoment);
    }
}
