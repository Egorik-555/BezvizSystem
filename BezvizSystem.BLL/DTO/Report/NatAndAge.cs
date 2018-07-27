using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO.Report
{
    public class NatAndAge
    {
        public string Natiolaty { get; set; }
        public int? ManLess18 { get; set; }
        public int? ManMore18 { get; set; }
        public int? WomanLess18 { get; set; }
        public int? WomanMore18 { get; set; }
        public int? All { get; set; }
    }
}
