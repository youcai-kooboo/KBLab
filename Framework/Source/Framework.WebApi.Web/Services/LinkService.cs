using System;
using Framework.WebApi.Common;
using Framework.WebApi.Hal;
using Framework.WebApi.Web.Controllers;

namespace Framework.WebApi.Web.Services
{
    public static class LinkService
    {
        public static void AddLink(this Representation representation, Type controllerType, ApiContext context, string name = null, string path = null)
        {
            if (controllerType == typeof(EntryPageController))
            {
                representation.AddLink(name ?? "self",
                                       ApiType.RootApi,
                                       path ?? String.Empty,
                                       context);
            }
            //else if (controllerType == typeof (SavingGoalController))
            //{
            //    Link savingGoalGetLink = representation.AddLink<SavingGoalController>(name ?? "saving-goal:get a saving goal",
            //                                                                        ApiType.RootApi,
            //                                                                        path ?? "{MemberID}",
            //                                                                        context);
            //    savingGoalGetLink.Data = new List<LinkQueryParameter>()
            //        {
            //            new LinkQueryParameter("MemberID", "Menzis MemberID, put it in url when sending.")
            //        };

            //    Link savingGoalPostLink = representation.AddLink<SavingGoalController>(name ?? "saving-goal:set a saving goal",
            //                                                     ApiType.RootApi,
            //                                                     path ?? "{MemberID}",
            //                                                     context);

            //     savingGoalPostLink.Method = HttpMethod.Post;
            //     savingGoalPostLink.Data = new List<LinkQueryParameter>()
            //        {
            //            new LinkQueryParameter("MemberID", "Menzis MemberID, put it in url when sending."),
            //            new LinkQueryParameter("Type","Points of Product,Values:Product or Points, put it in body when sending."),
            //            new LinkQueryParameter("Points", "Amount of points case type = Points  (else NULL), put it in body when sending."),
            //            new LinkQueryParameter("ProductID", "Product ID case type = Product  (else NULL), put it in body when sending.")
            //        };
            //}
           
        }
    }
}