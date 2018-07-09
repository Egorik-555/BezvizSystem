using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Entities.Loging
{
    public class UserActivity
    {
        public int Id { get; set; }

        public string Login { get; set; }
        public TypeOfOperation Operation { get; set; }
        public string Ip { get; set; }

        DateTime? TimeActivityFrom { get; set; }
        DateTime? TimeActivityTo { get; set; }
    }
}
