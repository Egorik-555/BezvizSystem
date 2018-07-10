using BezvizSystem.BLL.DTO;
using BezvizSystem.BLL.DTO.Log;
using BezvizSystem.BLL.Interfaces;
using BezvizSystem.BLL.Services;
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
            Bind<IService<AnketaDTO>>().To<AnketaService>();
            Bind<ILogger<UserActivityDTO>>().To<ActivityLoggerService>();
            Unbind<ModelValidatorProvider>();
        }
    }
}