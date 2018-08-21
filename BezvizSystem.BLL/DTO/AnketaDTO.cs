using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO
{
    public class AnketaDTO
    {
        public int Id { get; set; }
         
        public DateTime? DateArrival { get; set; }
        public int? DaysOfStay { get; set; }
        public string CheckPoint { get; set; }
        public int CountMembers { get; set; }
        public string Status { get; set; }
        public string Arrived { get; set; }
        public string Operator { get; set; }

        public ICollection<VisitorDTO> Visitors { get; set; }

        //public DateTime? DateInSystem { get; set; }
        //public string UserInSystem { get; set; }
      
        public AnketaDTO()
        {
            Visitors = new List<VisitorDTO>();
        }
    }
}
