using BezvizSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO
{
    public class GroupVisitorDTO
    {
        public int Id { get; set; }

        public virtual ICollection<VisitorDTO> Visitors { get; set; }

        public string UserUserName { get; set; }
        public string UserOperatorProfileUNP { get; set; }
        public string UserOperatorProfileTranscript { get; set; }
        public string StatusName { get; set; }
        public bool Group { get; set; }

        public DateTime? DateArrival { get; set; }
        public DateTime? DateDeparture { get; set; }
        public int? DaysOfStay { get; set; }
        public string CheckPoint { get; set; }
        public string PlaceOfRecidense { get; set; }

        public string ProgramOfTravel { get; set; }
        public string OrganizeForm { get; set; }
        public string Name { get; set; }
        public string NumberOfContract { get; set; }
        public DateTime? DateOfContract { get; set; }
        public string OtherInfo { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }

        public GroupVisitorDTO()
        {
            Visitors = new List<VisitorDTO>();
        }
    }
}
