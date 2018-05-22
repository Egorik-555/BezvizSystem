using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Operator
{
    public class ViewOperatorModel
    {
        public string Id { get; set; }       
        
        [Display(Name = "Туроператор")]
        public string ProfileUserTranscript { get; set; }
        [Display(Name = "УНП")]
        public string ProfileUserUNP { get; set; }
        [Display(Name = "ОКПО")]
        public string ProfileUserOKPO { get; set; }
    }
}