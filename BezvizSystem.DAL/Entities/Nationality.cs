using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Entities
{
    public class Nationality
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
