using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BezvizSystem.BLL.DTO.Dictionary
{
    public class NationalityDTO : DictionaryDTO
    {   
        public int Code { get; set; }      
        public string ShortName { get; set; }
    }
}
