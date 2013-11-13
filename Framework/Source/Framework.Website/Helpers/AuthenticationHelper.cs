using System;
using Framework.Website.Areas.Admin.Models;
using System.Web.Security;
using WebMatrix.WebData;

namespace Framework.Website.Helpers
{
    public static class AuthenticationHelper
    {
        public static void CreateUserAccountWithRoleAndLogin(RegisterModel model)
        {
            //For creating account in db
            WebSecurity.CreateUserAndAccount(model.UserName, model.Password,
                                             new
                                                 {
                                                     IsActive = true
                                                 });

            ////For login with created account
            //WebSecurity.Login(model.UserName, model.Password);

            //For assigning role to new account
            if (!Roles.RoleExists(model.UserRole))
            {
                Roles.CreateRole(model.UserRole);
            }
            Roles.AddUserToRole(model.UserName, model.UserRole);
        }

        public static void UpdateUserAccount(EditModel model)
        {
            if (Roles.GetRolesForUser(model.UserName).Length > 0)
            {
                Roles.RemoveUserFromRoles(model.UserName, Roles.GetRolesForUser(model.UserName));
            }

            Roles.AddUserToRole(model.UserName, model.UserRole);

            //change password
            if (!String.IsNullOrEmpty(model.Password))
            {
                MembershipUser u = Membership.GetUser(model.UserName);

                string token = WebSecurity.GeneratePasswordResetToken(model.UserName);
                WebSecurity.ResetPassword(token, model.Password);
            }
        }

        public static void DeleteUserAccount(string userName)
        {
           
            if (Roles.GetRolesForUser(userName).Length > 0)
            {
                Roles.RemoveUserFromRoles(userName, Roles.GetRolesForUser(userName));
            }
            ((SimpleMembershipProvider) Membership.Provider).DeleteAccount(userName);
            // deletes record from webpages_Membership table
            ((SimpleMembershipProvider) Membership.Provider).DeleteUser(userName, true);
            // deletes record from UserProfile table

        }
    }
}
