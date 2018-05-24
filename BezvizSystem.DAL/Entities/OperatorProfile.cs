using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Entities
{
    public class OperatorProfile
    {
        [Key]
        [ForeignKey("BezvizUser")]
        public string Id { get; set; }

        public virtual BezvizUser BezvizUser { get; set; }

        public string UNP { get; set; }
        public string OKPO { get; set; }
        public string Transcript { get; set; }
        public string Role { get; set; }
        public bool Active { get; set; }
        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
    }
}
