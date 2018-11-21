using BezvizSystem.BLL.Infrastructure;
using BezvizSystem.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Pogranec.Web.Infrastructure
{
    public static class HelperExtentions
    {
        public static bool IsRole(this IPrincipal principal, params UserLevel[] levels)
        {
            foreach(var level in levels)
            {
                if (principal.IsInRole(level.ToString())) return true;
            }

            return false;
        }
    }
}