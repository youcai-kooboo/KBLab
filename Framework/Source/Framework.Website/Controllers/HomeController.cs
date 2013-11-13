using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Data.Service;

namespace Framework.Website.Controllers
{
    public class HomeController : Controller
    {

        #region Dependency Injection

        private readonly IUserProfileService userService;
        public HomeController(IUserProfileService _userService)
        {
            this.userService = _userService;
        }
        #endregion


        [OutputCache(Duration = 10, VaryByParam = "none")]
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
