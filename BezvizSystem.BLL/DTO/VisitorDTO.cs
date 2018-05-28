﻿using BezvizSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO
{
    public  class VisitorDTO
    {
        public int Id { get; set; }

        public virtual GroupVisitorDTO Group { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string SerialAndNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? BithDate { get; set; }

        public string Nationality { get; set; }
    
        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
    }
}
