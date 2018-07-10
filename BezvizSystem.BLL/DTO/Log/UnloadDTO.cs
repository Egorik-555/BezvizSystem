using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO.Log
{
    public class UnloadDTO
    {
        public int Id { get; set; }
        public GroupVisitorDTO GroupVisitor { get; set; }
        public DateTime? DateUnload { get; set; }
        public string UserUnload { get; set; }
    }
}
