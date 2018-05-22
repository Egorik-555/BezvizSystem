using BezvizSystem.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BezvizSystem.Web.Helpers
{
    public static class Exstentions
    {
        public static string GetRole(this System.Security.Principal.IIdentity identity, IUserService service)
        {
            //if (service == null)
            //    return "guest";

            //var user = service.GetByNameAsync(identity.Name);
            //if (user != null)
            //    return user.Role;
            //else
                    return "guest";
        }
    }
}