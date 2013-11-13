using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Website.Filters;

namespace Framework.Website.Areas.Admin.Controllers
{
    [Authorize]
    [ControllerFilter]
    public class BaseController : Controller
    {
        public BaseController()
        {
             
        }
    }
}
