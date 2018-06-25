using BezvizSystem.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.Interfaces
{
    public interface IReport
    {
        ReportDTO GetReport(DateTime dateFrom, DateTime dateTo);       
    }
}
