using BezvizSystem.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.DAL.Entities
{
    public class XMLDispatch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public Status Status { get; set; }
        public Operation Operation { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }

        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }

        public override bool Equals(object obj)
        {
            var dispatch = obj as XMLDispatch;
            if (dispatch == null) return false;

            return (dispatch.Id == this.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
