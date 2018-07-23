using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Entities;

namespace BezvizSystem.BLL.Services
{
    public class ReportService : IReport
    {
        IUnitOfWork _database;
        IEnumerable<GroupVisitor> _groups;

        public ReportService(IUnitOfWork database)
        {
            this._database = database;
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public ReportDTO GetReport()
        {
            return GetReport(DateTime.Now.Date, DateTime.Now.Date);
        }

        public ReportDTO GetReport(DateTime? dateMoment)
        {
            return GetReport(DateTime.Now.Date, DateTime.Now.Date, dateMoment);
        }

        public ReportDTO GetReport(DateTime? dateFrom, DateTime? dateTo)
        {
            return GetReport(dateFrom, dateTo, DateTime.Now.Date);
        }

        public ReportDTO GetReport(DateTime? dateFrom, DateTime? dateTo, DateTime? dateMoment)
        {           
            _groups = _database.GroupManager.GetAll().Where(g => g.DateArrival >= dateFrom && g.DateArrival <= dateTo).ToList();

            return new ReportDTO
            {
                DateFrom = dateFrom.Value.ToShortDateString(),
                DateTo = dateTo.Value.ToShortDateString(),

                AllRegistrated = GetAllRegistrated().ToString(),
                AllArrived = GetAllArrived().ToString(),
                WaitArrived = GetWaitArrived(dateMoment).ToString(),
                NotArriverd = GetNotArrived(dateMoment).ToString(),
                AllTourist = GetAllTourist().ToString(),
                AllGroup = GetAllGroup().ToString(),
                AllTouristInGroup = GetAllTouristInGroup().ToString()
            };
        }

        private int? GetAllRegistrated()
        {
            return _groups.Select(a => a.Visitors.Count).Sum();
        }

        private int? GetAllArrived()
        {        
            return _groups.Select(a => a.Visitors.Where(v => v.Arrived).Count()).Sum();
        }

        private int? GetWaitArrived(DateTime? dateMoment)
        {
            var groups = _groups.Where(g => g.DateArrival >= dateMoment).ToList();
            return groups.Select(a => a.Visitors.Where(v => !v.Arrived).Count()).Sum();
        }

        private int? GetNotArrived(DateTime? dateMoment)
        {
            var groups = _groups.Where(g => g.DateArrival < dateMoment);
            return groups.Select(a => a.Visitors.Where(v => !v.Arrived).Count()).Sum();
        }

        private int? GetAllTourist()
        {
            var groups = _groups.Where(g => !g.Group);
            return groups.Select(a => a.Visitors.Count()).Sum();
        }

        private int? GetAllGroup()
        {
            var groups = _groups.Where(g => g.Group);
            return groups.Select(a => a.Id).Count();
        }    

        private int? GetAllTouristInGroup()
        {
            var groups = _groups.Where(g => g.Group);
            return groups.Select(a => a.Visitors.Count()).Sum();
        }            
    }
}
