using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO
{
    public class ProfileUserDTO
    {
        public string Id { get; set; }

        public string UNP { get; set; }
        public string OKPO { get; set; }
        public string Transcript { get; set; }
        public string Ip { get; set; }
        public bool Active { get; set; }
        public string Address { get; set; }
        public DateTime? NotActiveToDate { get; set; }
        public string Role { get; set; }
        
        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }
    }
}
