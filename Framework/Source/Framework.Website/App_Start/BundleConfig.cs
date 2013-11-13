using System.Web;
using System.Web.Optimization;

namespace Framework.Website
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                          "~/Scripts/jquery-1.10.2.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-1.8.24.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                         "~/Scripts/jquery.unobtrusive*",
                         "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.6.2.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap/js").Include(
                "~/Content/Bootstrap/js/bootstrap.js"));
            bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include(
                "~/Content/Bootstrap/css/bootstrap.css",
                "~/Content/Bootstrap/css/bootstrap-theme.css"));

//            bundles.Add(new ScriptBundle("~/bundles/bootstrap/js").Include(
//                        "~/Content/Bootstrap/js/bootstrap-paginator.min.js"
//,                       "~/Content/Bootstrap/js/bootstrap-transition.js",
//                        "~/Content/Bootstrap/js/bootstrap-alert.js",
//                        "~/Content/Bootstrap/js/bootstrap-modal.js",
//                        "~/Content/Bootstrap/js/bootstrap-modalmanager.js",
//                        "~/Content/Bootstrap/js/bootstrap-dropdown.js",
//                        "~/Content/Bootstrap/js/bootstrap-scrollspy.js",
//                        "~/Content/Bootstrap/js/bootstrap-tab.js",
//                        "~/Content/Bootstrap/js/bootstrap-tooltip.js",
//                        "~/Content/Bootstrap/js/bootstrap-popover.js",
//                        "~/Content/Bootstrap/js/bootstrap-button.js",
//                        "~/Content/Bootstrap/js/bootstrap-collapse.js",                       
//                        "~/Content/Bootstrap/js/bootstrap-carousel.js",
//                        "~/Content/Bootstrap/js/bootstrap-typeahead.js",
//                        "~/Content/Bootstrap/js/bootstrap-tour.js",
//                        "~/Content/Bootstrap/js/jquery.cookie.js",
//                        "~/Content/Bootstrap/js/fullcalendar.min.js",
//                        "~/Content/Bootstrap/js/jquery.dataTables.min.js",
//                        "~/Content/Bootstrap/js/bootstrap-toggle.js",
//                        "~/Content/Bootstrap/js/jquery.dataTables.min.js",
//                        "~/Content/Bootstrap/js/excanvas.js",
//                        "~/Content/Bootstrap/js/jquery.flot.min.js",
//                        "~/Content/Bootstrap/js/jquery.flot.pie.min.js",
//                        "~/Content/Bootstrap/js/jquery.flot.resize.min.js",
//                        "~/Content/Bootstrap/js/jquery.flot.stack.js",
//                        "~/Content/Bootstrap/js/jquery.chosen.min.js",
//                        "~/Content/Bootstrap/js/jquery.uniform.min.js",
//                        "~/Content/Bootstrap/js/jquery.colorbox.min.js",
//                        "~/Content/Bootstrap/js/jquery.cleditor.min.js",
//                        "~/Content/Bootstrap/js/jquery.noty.js",
//                        "~/Content/Bootstrap/js/jquery.elfinder.min.js",
//                        "~/Content/Bootstrap/js/jquery.raty.min.js",
//                        "~/Content/Bootstrap/js/jquery.iphone.toggle.js",
//                        "~/Content/Bootstrap/js/jquery.autogrow-textarea.js",                                               
//                        "~/Content/Bootstrap/js/jquery.uploadify-3.1.min.js",                      
//                        "~/Content/Bootstrap/js/jquery.history.js",
//                        "~/Content/Bootstrap/js/charisma.js"));

//            bundles.Add(new StyleBundle("~/bundles/bootstrap/css").Include(
//                       "~/Content/Bootstrap/css/bootstrap-spacelab.css",
//                       "~/Content/Bootstrap/css/bootstrap-responsive.css",
//                       "~/Content/Bootstrap/css/charisma-app.css",
//                       "~/Content/Bootstrap/css/jquery-ui-1.8.21.custom.css",
//                       "~/Content/Bootstrap/css/fullcalendar.css",
//                       "~/Content/Bootstrap/css/fullcalendar.print.css",
//                       "~/Content/Bootstrap/css/chosen.css",
//                       "~/Content/Bootstrap/css/uniform.default.css",
//                       "~/Content/Bootstrap/css/colorbox.css",
//                       "~/Content/Bootstrap/css/jquery.cleditor.css",
//                       "~/Content/Bootstrap/css/jquery.noty.css",
//                       "~/Content/Bootstrap/css/noty_theme_default.css",
//                       "~/Content/Bootstrap/css/elfinder.min.css",
//                       "~/Content/Bootstrap/css/elfinder.theme.css",
//                       "~/Content/Bootstrap/css/jquery.iphone.toggle.css",
//                       "~/Content/Bootstrap/css/opa-icons.css",
//                       "~/Content/Bootstrap/css/uploadify.css"));
        }
    }
}