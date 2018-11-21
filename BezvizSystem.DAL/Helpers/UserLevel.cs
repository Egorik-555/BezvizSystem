using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BezvizSystem.DAL.Helpers
{
    public enum UserLevel
    {
        OBLSuperAdmin = 1,
        OBLAdmin = 2,
        OBLUser = 3,

        GPKSuperAdmin = 10,
        GPKAdmin = 11,
        GPKMiddle = 12,
        GPKUser = 13
    }
}