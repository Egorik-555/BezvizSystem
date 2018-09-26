using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Entities;
using BezvizSystem.BLL.Report.DTO;
using BezvizSystem.BLL.DTO.Report;
using AutoMapper;
using BezvizSystem.BLL.Mapper;
using System.Web.Helpers;
using Newtonsoft.Json;
using System.Web;

namespace BezvizSystem.BLL.Services
{
    public class ReportService : IReport
    {
        IUnitOfWork _database;
        IEnumerable<GroupVisitor> _groups;
        IEnumerable<Visitor> _visitors;
        IMapper _mapper;

        public ReportService(IUnitOfWork database)
        {
            this._database = database;
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new ReportServiceProfile())).CreateMapper();
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
            _visitors = _database.VisitorManager.GetAll().Where(v => v.Group != null && v.Group.DateArrival >= dateFrom && v.Group.DateArrival <= dateTo).ToList();

            return new ReportDTO
            {
                DateFrom = dateFrom.Value,
                DateTo = dateTo.Value,

                AllRegistrated = GetAllRegistrated(),
                AllArrived = GetAllArrived(),
                WaitArrived = GetWaitArrived(dateMoment),
                NotArriverd = GetNotArrived(dateMoment),
                AllTourist = GetAllTourist(),
                AllGroup = GetAllGroup(),
                AllTouristInGroup = GetAllTouristInGroup(),

                AllByNatAndAge = GetByNatAndAge(dateMoment),
                //AllByDateArrivalCount = GetByDateArrivalCount(),
                AllByCheckPointCount = GetByCheckPointCount(),
                AllByDaysCount = GetByDaysCount(),
                AllByOperatorCount = GetByOperatorCount(),

                StringDateByArrivalCount = GetByDateArrivalCount()
            };
        }

        private IEnumerable<NatAndAge> GetByNatAndAge(DateTime? dateMoment)
        {
            var visitors = _visitors.GroupBy(v => v.Nationality.ShortName)
                                                            .Select(g =>
                                                                new NatAndAge
                                                                {
                                                                    Natiolaty = g.Key,
                                                                    ManLess18 = g.Where(v => v.Gender.Code == 1 && GetDiffDatesInYears(dateMoment, v.BithDate) < 18
                                                                                                                 && GetDiffDatesInYears(dateMoment, v.BithDate) >= 0).Count(),
                                                                    ManMore18 = g.Where(v => v.Gender.Code == 1 && GetDiffDatesInYears(dateMoment, v.BithDate) >= 18).Count(),
                                                                    WomanLess18 = g.Where(v => v.Gender.Code == 2 && GetDiffDatesInYears(dateMoment, v.BithDate) < 18
                                                                                                                   && GetDiffDatesInYears(dateMoment, v.BithDate) >= 0).Count(),
                                                                    WomanMore18 = g.Where(v => v.Gender.Code == 2 && GetDiffDatesInYears(dateMoment, v.BithDate) >= 18).Count(),

                                                                    All = g.Count()
                                                                });

            List<NatAndAge> list = new List<NatAndAge>(visitors);
            return list;
        }

        private string GetString(string label1, string label2, IEnumerable<ObjectForDiagram> list)
        {
            string result = "{\"cols\" : [";
            result += "{\"id\":\"\",\"label\":\"" + label1 + "\",\"pattern\":\"\",\"type\":\"string\"},";
            result += "{\"id\":\"\",\"label\":\"" + label2 + "\",\"pattern\":\"\",\"type\":\"number\"}";
            result += "],";
            result += "\"rows\": [";

            foreach (var item in list)
            {
                result += "{ \"c\":[{\"v\":\"" + item.Value1 + "\",\"f\":null},{\"v\":" + item.Value2 + ",\"f\":null}]},";
            }

            result += "] }";

            return result;
        }

        //private IEnumerable<CountByDate> GetByDateArrivalCount()
        //{
        //    var visitors = _visitors.GroupBy(v => v.Group.DateArrival.Value.Date).Select(g => new CountByDate { DateArrival = g.Key.Date, Count = g.Count()});
        //    return visitors.OrderBy(o => o.DateArrival).ToList();
        //}

        private string GetByDateArrivalCount()
        {
            var visitors = _visitors.GroupBy(v => v.Group.DateArrival.Value.Date).Select(g => new CountByDate { DateArrival = g.Key.Date, Count = g.Count() });
            visitors = visitors.OrderBy(o => o.DateArrival).ToList();

            var list = _mapper.Map<IEnumerable<CountByDate>, IEnumerable<ObjectForDiagram>>(visitors);

            var res = HttpUtility.JavaScriptStringEncode(@"Hello """"");
            return res;
        }

        private IEnumerable<CountByCheckPoint> GetByCheckPointCount()
        {
            var visitors = _visitors.GroupBy(v => v.Group.CheckPoint.Name).Select(g => new CountByCheckPoint { CheckPoint = g.Key, Count = g.Count() });
            return visitors.ToList();
        }

        private IEnumerable<CountByDays> GetByDaysCount()
        {
            var visitors = _visitors.GroupBy(v => v.Group.DaysOfStay).Select(g => new CountByDays { Days = g.Key, Count = g.Count() });
            return visitors.ToList();
        }

        private IEnumerable<CountByOperator> GetByOperatorCount()
        {
            var visitors = _visitors.GroupBy(v => v.Group.TranscriptUser).Select(g => new CountByOperator { Operator = g.Key, Count = g.Count() });
            return visitors.ToList();
        }

        private int GetDiffDatesInYears(DateTime? date1, DateTime? date2)
        {
            int result = 0;
            if (date1.HasValue && date2.HasValue)
                result = (date1.Value.Date - date2.Value.Date).Days / 365;
            else result = -1;

            return result;
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
