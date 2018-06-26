using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.DAL.Interfaces;

namespace BezvizSystem.BLL.Services
{
    public class ReportService : IReport
    {
        IUnitOfWork _database;
        DateTime _dateFrom;
        DateTime _dateTo;

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

        public ReportDTO GetReport(DateTime dateFrom)
        {
            return GetReport(dateFrom, DateTime.Now.Date);
        }

        public ReportDTO GetReport(DateTime dateFrom, DateTime dateTo)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;

            //var groups = _database.GroupManager.GetAll();
            //int? count = GetAllTouristInGroup();

            return new ReportDTO
            {
                DateFrom = _dateFrom.ToShortDateString(),
                DateTo = _dateTo.ToShortDateString(),

                AllRegistrated = GetAllRegistrated().ToString(),
                AllArrived = GetAllArrived().ToString(),
                WaitArrived = GetWaitArrived().ToString(),
                NotArriverd = GetNotArrived().ToString(),
                AllTourist = GetAllTourist().ToString() + " чел.",
                AllGroup = GetAllGroup().ToString() + " (" + GetAllTouristInGroup() + " чел.)"
            };
        }

        private int? GetAllArrived()
        {
            var groups = _database.GroupManager.GetAll().Where(g => g.DateArrival >= _dateFrom && g.DateArrival <= _dateTo);
            return groups.Select(a => a.Visitors.Where(v => v.Arrived).Count()).Sum();
        }

        private int? GetAllGroup()
        {
            var groups = _database.GroupManager.GetAll().Where(g => g.Group).Where(g => g.DateArrival >= _dateFrom && g.DateArrival <= _dateTo).ToList();
            return groups.Select(a => a.Id).Count();
        }

        private int? GetAllRegistrated()
        {
            var groups = _database.GroupManager.GetAll().Where(g => g.DateArrival >= _dateFrom && g.DateArrival <= _dateTo);
            return groups.Select(a => a.Visitors.Count).Sum();
        }

        private int? GetAllTourist()
        {
            var groups = _database.GroupManager.GetAll().Where(g => !g.Group).Where(g => g.DateArrival >= _dateFrom && g.DateArrival <= _dateTo);
            return groups.Select(a => a.Visitors.Count()).Sum();
        }

        private int? GetAllTouristInGroup()
        {
            var groups = _database.GroupManager.GetAll().Where(g => g.Group).Where(g => g.DateArrival >= _dateFrom && g.DateArrival <= _dateTo);
            return groups.Select(a => a.Visitors.Count()).Sum();
        }

        private int? GetNotArrived()
        {
            var groups = _database.GroupManager.GetAll().Where(g => g.DateArrival >= _dateFrom && g.DateArrival <= _dateTo);
            groups = groups.Where(g => g.DateArrival < DateTime.Now).ToList();
            return groups.Select(a => a.Visitors.Where(v => !v.Arrived).Count()).Sum();
        }

        private int? GetWaitArrived()
        {
            var groups = _database.GroupManager.GetAll().Where(g => g.DateArrival >= _dateFrom && g.DateArrival <= _dateTo);
            groups = groups.Where(g => g.DateArrival >= DateTime.Now).ToList();
            return groups.Select(a => a.Visitors.Where(v => !v.Arrived).Count()).Sum();
        }
    }
}
