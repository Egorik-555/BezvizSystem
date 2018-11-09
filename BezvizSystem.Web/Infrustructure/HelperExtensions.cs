using BezvizSystem.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BezvizSystem.Web.Infrustructure
{
    public static class HelperExtensions
    {
        public static bool IsRole(this IPrincipal principal, params UserLevel[] levels)
        {
            foreach (var level in levels)
            {
                if (principal.IsInRole(level.ToString())) return true;
            }

            return false;
        }
    }
}