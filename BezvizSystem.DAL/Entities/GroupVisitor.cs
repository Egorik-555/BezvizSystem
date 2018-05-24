using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Entities
{
    public class GroupVisitor
    {
        public int Id { get; set; }

        public virtual ICollection<Visitor> Visitors { get; set; }
        public virtual BezvizUser User { get; set; }
        public virtual Status Status { get; set; }

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

        public string TimeOfWork { get; set; }
        public string SiteOfOperator { get; set; }
        public string TelNumber { get; set; }
        public string Email { get; set; }

        public string OtherInfo { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }

        public GroupVisitor()
        {
            Visitors = new List<Visitor>();
        }
    }
}
