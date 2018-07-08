using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Entities.Loging
{
    public class Unload
    {
        public int Id { get; set; }
        public GroupVisitor GroupVisitor { get; set; }
        public DateTime? DateUnload { get; set; }
        public string UserUnload { get; set; }
    }
}
