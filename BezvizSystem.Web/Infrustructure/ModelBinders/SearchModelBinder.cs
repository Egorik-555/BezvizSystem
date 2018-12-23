using BezvizSystem.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BezvizSystem.Web.Infrustructure.ModelBinders
{
    public class SearchModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProvider = bindingContext.ValueProvider;
            var value = valueProvider.GetValue("search");
            if (value != null)
            {
                var str = (string)value.ConvertTo(typeof(string));
                var model = JsonConvert.DeserializeObject<SearchModel>(str);
                model.DateFrom = model.DateFrom.HasValue ? model.DateFrom.Value.ToLocalTime() : DateTime.MinValue;
                model.DateTo = model.DateTo.HasValue ? model.DateTo.Value.ToLocalTime() : DateTime.MaxValue;

                return model;
            }

            return base.BindModel(controllerContext, bindingContext);
        }       
    }
}