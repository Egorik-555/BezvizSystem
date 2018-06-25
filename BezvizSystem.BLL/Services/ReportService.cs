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

        public ReportDTO GetReport(DateTime dateFrom, DateTime dateTo)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;

            var groups = _database.GroupManager.GetAll();
            int? count = GetWaitArrived();

            return null;
        }

        private int? GetAllArrived()
        {
            var groups = _database.GroupManager.GetAll();
            return groups.Select(a => a.Visitors.Where(v => v.Arrived).Count()).Sum();
        }

        //private int? GetAllGroup()
        //{
            


        //}

        private int? GetAllRegistrated()
        {
            var groups = _database.GroupManager.GetAll();
            return groups.Select(a => a.Visitors.Count).Sum();
        }

        //private int? GetAllTourist)
        //{
            
        //}

        //private int? GetAllTouristInGroup()
        //{
            
        //}

        //private int? GetNotArrived()
        //{
            
        //}

        private int? GetWaitArrived()
        {
            var groups = _database.GroupManager.GetAll();

            groups = groups.Where(g => g.DateArrival >= DateTime.Now).ToList();

            return groups.Select(a => a.Visitors.Count).Sum();
        }     
    }
}
