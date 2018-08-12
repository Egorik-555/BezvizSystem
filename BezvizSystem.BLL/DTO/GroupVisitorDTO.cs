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
        public GroupVisitorDTO()
        {
            Visitors = new List<VisitorDTO>();
        }

        public int Id { get; set; }

        public virtual ICollection<VisitorDTO> Visitors { get; set; }

        public bool Group { get; set; }

        public DateTime? DateArrival { get; set; }
        public DateTime? DateDeparture { get; set; }
        public int? DaysOfStay { get; set; }
        public string CheckPoint { get; set; }
        public string PlaceOfRecidense { get; set; }
        public string ProgramOfTravel { get; set; }

        //part of visitor only
        public string TimeOfWork { get; set; }
        public string SiteOfOperator { get; set; }
        public string TelNumber { get; set; }
        public string Email { get; set; }
        //

        //part of visitor group only
        public string OrganizeForm { get; set; }
        public string Name { get; set; }
        public string NumberOfContract { get; set; }
        public DateTime? DateOfContract { get; set; }
        public string OtherInfo { get; set; }
        //

        public bool ExtraSend { get; set; }
        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
        public string TranscriptUser { get; set; }
        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }

        public override bool Equals(object obj)
        {
            var group = obj as GroupVisitorDTO;
            if (group == null) return false;

            if (group.Id != this.Id) return false;

            return true;
        }
      
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
