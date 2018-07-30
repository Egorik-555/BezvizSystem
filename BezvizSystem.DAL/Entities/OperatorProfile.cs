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
        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }

        public override bool Equals(object obj)
        {
            OperatorProfile profile = obj as OperatorProfile;
            if (profile == null) return false;

            if (profile.Id != this.Id) return false;

            if (profile.OKPO == null && OKPO != null) return false;
            if (profile.OKPO != null && OKPO == null) return false;
            if (profile.OKPO != null && OKPO != null)
                if (profile.OKPO != OKPO) return false;

            if (profile.UNP == null && UNP != null) return false;
            if (profile.UNP != null && UNP == null) return false;
            if (profile.UNP != null && UNP != null)
                if (profile.UNP != UNP) return false;

            if (profile.Transcript == null && Transcript != null) return false;
            if (profile.Transcript != null && Transcript == null) return false;
            if (profile.Transcript != null && Transcript != null)
                if (profile.Transcript != Transcript) return false;


            if (profile.Role == null && Role != null) return false;
            if (profile.Role != null && Role == null) return false;
            if (profile.Role != null && Role != null)
                if (profile.Role != Role) return false;

          
            if (profile.Active != Active) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
