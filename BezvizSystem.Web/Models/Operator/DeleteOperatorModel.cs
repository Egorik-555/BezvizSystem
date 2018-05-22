using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Operator
{
    public class DeleteOperatorModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        [Display(Name = "Наименование")]
        public string ProfileUserTranscript { get; set; }
        [Display(Name = "УНП")]
        public string ProfileUserUNP { get; set; }
    }
}