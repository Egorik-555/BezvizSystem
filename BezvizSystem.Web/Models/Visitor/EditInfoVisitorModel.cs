﻿using BezvizSystem.Web.Infrustructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Models.Visitor
{
    public class EditInfoVisitorModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Серия и номер паспорта")]
        public string SerialAndNumber { get; set; }

        [Required]
        [Display(Name = "Пол")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Дата рождения не может быть будущей")]
        public DateTime? BithDate { get; set; }

        [Required]
        [Display(Name = "Гражданство")]
        public string Nationality { get; set; }

        public bool Arrived { get; set; }

        public DateTime? DateInSystem { get; set; }
        public string UserInSystem { get; set; }
        public DateTime? DateEdit { get; set; }
        public string UserEdit { get; set; }

    }
}