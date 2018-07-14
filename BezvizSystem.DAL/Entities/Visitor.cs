using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Entities
{
    public class Visitor
    {     
        public int Id { get; set; }
       
        public virtual GroupVisitor Group { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string SerialAndNumber { get; set; }
        public virtual Gender Gender { get; set; }
        public DateTime? BithDate { get; set; }
        public virtual Nationality Nationality { get; set; }    

        public bool Arrived { get; set; }
    
        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }
    }
}
