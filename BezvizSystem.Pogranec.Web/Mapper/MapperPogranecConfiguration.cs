using AutoMapper;
using AutoMapper.Configuration;
using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.Report.DTO;
using BezvizSystem.Pogranec.Web.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                cfg.CreateMap<ReportModel, ReportDTO>();

                cfg.CreateMap<BLL.DTO.Report.NatAndAge, ViewTable1InExcel>();
            })
        {   
        }
    }
}
