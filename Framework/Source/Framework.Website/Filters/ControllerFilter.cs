using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.Website.Filters
{
    public class ControllerFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //if (filterContext.Controller.GetType() != typeof(CustomersController))
            //{
            //    filterContext.HttpContext.Session["SearchModel"] = null;
            //    filterContext.HttpContext.Session["EmailNames"] = null;
            //    filterContext.HttpContext.Session["Kanaals"] = null;
            //    filterContext.HttpContext.Session["NaamAfzenders"] = null;
            //}
            base.OnActionExecuted(filterContext);
        }
    }
}