using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Framework.Data.Model;
using Framework.Data.Service;
using Framework.Website.Areas.Admin.Models;
using Framework.Website.Filters;
using Framework.Website.Helpers;
using WebMatrix.WebData;

namespace Framework.Website.Areas.Admin.Controllers
{
    [InitializeSimpleMembership]
    public class UserController : BaseController
    {

        #region Dependency Injection

        private readonly IUserProfileService userService;
        public UserController(IUserProfileService _userService)
        {
            this.userService = _userService;
        }
        #endregion


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                string userRole = Roles.GetRolesForUser(model.UserName).FirstOrDefault();

                return RedirectToAction("Index");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Login", "User");
        }

        public ActionResult Register()
        {
            SetUserRolesToViewData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    AuthenticationHelper.CreateUserAccountWithRoleAndLogin(model);

                    return RedirectToAction("Index", "User");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("Email", ErrorCodeToString(e.StatusCode));
                }
            }
            SetUserRolesToViewData();
            return View(model);
        }


        public ActionResult Index()
        {
            var userProfiles = userService.GetAllUsers().Where(m => m.UserName != "admin");
            ViewBag.UserProfiles = userProfiles;

            return View();
        }

        public ActionResult Delete(string userName)
        {
            SetUserRolesToViewData();
            UserProfile userProfile = userService.GetUserByUserName(userName);

            if (userProfile != null)
            {
                AuthenticationHelper.DeleteUserAccount(userName);
            }

            return RedirectToAction("Index", "User");
        }

        public ActionResult Edit(string userName)
        {
            UserProfile userProfile = userService.GetUserByUserName(userName);
            EditModel model = new EditModel()
            {
                UserName = userProfile.UserName,
                UserRole = Roles.GetRolesForUser(userProfile.UserName).FirstOrDefault()
            };
            SetUserRolesToViewData();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditModel model)
        {
            string role = model.UserRole;
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    AuthenticationHelper.UpdateUserAccount(model);

                    return RedirectToAction("Index", "User");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("Email", ErrorCodeToString(e.StatusCode));
                }
            }

            return View(model);
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";

            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            ViewBag.ReturnUrl = Url.Action("Manage");

            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword,
                                                                         model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers

        private void SetUserRolesToViewData()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Admin", Value = UserRole.Admin.ToString() });
            items.Add(new SelectListItem() { Text = "Statistics", Value = UserRole.Statistics.ToString() });
            items.Add(new SelectListItem() { Text = "Customer Overview", Value = UserRole.CustomerOverview.ToString() });
            items.Add(new SelectListItem() { Text = "Statistics And Customer Overview", Value = UserRole.StatisticsAndCustomerOverview.ToString() });

            ViewData["UserRoles"] = items;
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }



        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
