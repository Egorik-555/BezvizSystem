using BezvizSystem.Web.Models.Group;
using BezvizSystem.Web.Models.Visitor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Infrustructure
{
    public class DateArrivalVisitorLessAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {          
            CreateVisitorModel model = value as CreateVisitorModel;        
            if (model == null || model.DateArrival == null || model.DateDeparture == null)
                return true;

            return model.DateArrival <= model.DateDeparture;
        }
    }
}