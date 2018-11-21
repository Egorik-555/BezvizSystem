using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BezvizSystem.Pogranec.Web.Models.Log
{
    public class LogModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
       // public string Type { get; set; }

        public string Ip { get; set; }
        public DateTime? DateActivity { get; set; }
        public string TextActivity { get; set; }
    }
}