using BezvizSystem.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Identity
{
    public class BezvizRoleManager : RoleManager<BezvizRole>
    {
        public BezvizRoleManager(IRoleStore<BezvizRole> store)
            : base(store)
        {

        }

        public BezvizRoleManager(IRoleStore<BezvizRole, string> store) : base(store)
        {
        }
    }
}
