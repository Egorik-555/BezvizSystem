using AutoMapper;
using AutoMapper.Configuration;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.DTO.Report;
using BezvizSystem.BLL.Report.DTO;
using BezvizSystem.DAL.Helpers;
using BezvizSystem.Pogranec.Web.Models.Log;
using BezvizSystem.Pogranec.Web.Models.Report;
using System;

namespace BezvizSystem.Pogranec.Web.Mapper
{
    public class MapperPogranecConfiguration : MapperConfiguration
    {
        public MapperPogranecConfiguration(MapperConfigurationExpression configurationExpression)
            :base(configurationExpression)
        {

        }

        public MapperPogranecConfiguration(Action<IMapperConfigurationExpression> configure)
            :base(configure)
        {

        }

        public MapperPogranecConfiguration() 
            :base(cfg =>
            {
                cfg.CreateMap<ReportDTO, ReportModel>();
                cfg.CreateMap<NatAndAge, NatAndAgeModel>();
                cfg.CreateMap<CountByOperator, CountByOperatorModel>();
                cfg.CreateMap<CountByDate, CountByDateModel>().ForMember(dest => dest.DateArrival, 
                                                                         opt => opt.MapFrom(src => src.DateArrival.HasValue ? src.DateArrival.Value.ToShortDateString() : ""));
                cfg.CreateMap<CountByCheckPoint, CountByCheckPointModel>();
                cfg.CreateMap<CountByDays, CountByDaysModel>();

                cfg.CreateMap<ReportModel, ReportDTO>();

                cfg.CreateMap<NatAndAgeModel, NatAndAgeExcel>();

                cfg.CreateMap<LogDTO, LogModel>().ForMember(dest => dest.Type, opt => opt.MapFrom(src => LogTypeToString(src.Type)));
            })
        {   
        }

        static string LogTypeToString(LogType type)
        {
            switch (type)
            {
                case LogType.Enter:
                    return "Вход";
                case LogType.Exit:
                    return "Выход";
                case LogType.ExtraLoadXML:
                    return "Экстренная выгрузка xml";
                case LogType.LoadXML:
                    return "Выгорузка xml";
                case LogType.EditUser:
                    return "Редактирование пользователя";
                case LogType.CreateUser:
                    return "Создание пользователя";
                case LogType.DeleteUser:
                    return "Удаление пользователя";
                default:
                    return "Неопределнный тип операции";
            }
        }
    }
}
