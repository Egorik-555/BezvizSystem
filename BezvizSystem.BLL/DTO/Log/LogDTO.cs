using BezvizSystem.DAL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO.Log
{
    public class LogDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public LogType Type { get; set; }
        public UserLevel UserRole { get; set; }

        public string Ip { get; set; }
        public DateTime? DateActivity { get; set; }
        public string TextActivity { get; set; }
    }
}
