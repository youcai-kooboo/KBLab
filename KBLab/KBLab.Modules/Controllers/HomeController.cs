using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace KBLab.Modules.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new EmptyResult();
        }
    }
}
