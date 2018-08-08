using BezvizSystem.DAL;
using BezvizSystem.DAL.Entities;
using BezvizSystem.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO
{
    public  class VisitorDTO
    {
        public int Id { get; set; }

        public virtual GroupVisitorDTO Group { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string SerialAndNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? BithDate { get; set; }
        public string Nationality { get; set; }

        public StatusOfRecord StatusOfRecord { get; set; }
        public StatusOfOperation StatusOfOperation { get; set; }
        public bool Arrived { get; set; }
    
        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }

        public override bool Equals(object obj)
        {
            var visitor = obj as VisitorDTO;
            if (visitor == null) return false;

            if (visitor.Id != this.Id) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
