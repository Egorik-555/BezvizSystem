using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models
{
    public class SearchModel
    {
        public string Name { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string CheckPoint { get; set; }
    }
}