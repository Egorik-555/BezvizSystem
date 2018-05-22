using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "УНП")]
        public string UserName { get; set; }

        [Required]       
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}