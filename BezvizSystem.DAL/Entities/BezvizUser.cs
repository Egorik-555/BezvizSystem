using BezvizSystem.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL
{
    public class BezvizUser : IdentityUser
    {
        public BezvizUser()
        {
            Groups = new List<GroupVisitor>();
        }

        public virtual OperatorProfile OperatorProfile { get; set; }
        public virtual ICollection<GroupVisitor> Groups { get; set; }    
    }
}
