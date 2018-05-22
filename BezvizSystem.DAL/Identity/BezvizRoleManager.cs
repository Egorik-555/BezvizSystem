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
        public BezvizRoleManager(RoleStore<BezvizRole> store)
            : base(store)
        {

        }
    }
}
