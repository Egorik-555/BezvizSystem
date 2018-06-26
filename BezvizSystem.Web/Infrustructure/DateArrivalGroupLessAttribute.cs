using BezvizSystem.Web.Models.Group;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BezvizSystem.Web.Infrustructure
{
    public class DateArrivalGroupLessAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            CreateGroupModel model = value as CreateGroupModel;
            if (model == null || model.DateArrival == null || model.DateDeparture == null)
                return true;

            return model.DateArrival <= model.DateDeparture;
        }
    }
}