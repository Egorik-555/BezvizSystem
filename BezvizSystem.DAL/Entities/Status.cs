using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }

        public bool Active { get; set; }
        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
    }
}
