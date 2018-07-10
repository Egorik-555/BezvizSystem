using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO.Log
{
    public class UserActivityDTO
    {
        public int Id { get; set; }

        public string Login { get; set; }
        public string Operation { get; set; }
        public string Ip { get; set; }

        public DateTime? TimeActivity { get; set; }
    }
}
