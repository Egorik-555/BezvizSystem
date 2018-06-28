using BezvizSystem.Web.Models.Group;
using BezvizSystem.Web.Models.Visitor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Infrustructure
{
    public class DateArrivalLessAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {          
            dynamic model = value as CreateVisitorModel;
            if (model == null) model = value as EditVisitorModel;
            if (model == null) model = value as CreateGroupModel;
            if (model == null) model = value as EditGroupModel;

            if (model == null || model.DateArrival == null || model.DateDeparture == null)
                return true;

            return model.DateArrival <= model.DateDeparture;
        }
    }
}