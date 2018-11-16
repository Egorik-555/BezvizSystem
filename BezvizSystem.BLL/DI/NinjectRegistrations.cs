using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Dictionary;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Interfaces.Log;
using BezvizSystem.BLL.Interfaces.XML;
using BezvizSystem.BLL.Services;
using BezvizSystem.BLL.Services.Log;
using BezvizSystem.BLL.Services.XML;
using BezvizSystem.DAL.Interfaces;
using BezvizSystem.DAL.Repositories;
using Ninject.Modules;
using System.Web.Mvc;

namespace BezvizSystem.BLL.DI
{
    public class NinjectRegistrations : NinjectModule
    {
        const string CONNECTION = "BezvizContext";

        public override void Load()
        {
            Bind<IUnitOfWork>().To<IdentityUnitOfWork>().WithConstructorArgument("connection", CONNECTION);
            Bind<IUserService>().To<UserService>();
            Bind<IXMLDispatcher>().To<XMLDispatcher>();

            Bind<IService<VisitorDTO>>().To<VisitorService>();
            Bind<IService<GroupVisitorDTO>>().To<GroupService>();
            Bind<IService<AnketaDTO>>().To<AnketaService>();
            Bind<ILogger>().To<Logger>();

            Bind<IDictionaryService<CheckPointDTO>>().To<DictionaryService<CheckPointDTO>>();
            Bind<IDictionaryService<NationalityDTO>>().To<DictionaryService<NationalityDTO>>();
            Bind<IDictionaryService<GenderDTO>>().To<DictionaryService<GenderDTO>>();

            Bind<IXmlCreator>().To<XmlCreatorPogran>();
            Bind<IReport>().To<ReportService>();
            //Unbind<ModelValidatorProvider>();
        }
    }
}