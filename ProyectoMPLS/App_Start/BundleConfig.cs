using System.Web;
using System.Web.Optimization;

namespace ProyectoMPLS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/assets/js/jquery.min.js",
                        "~/assets/js/bootstrap.min.js",
                        "~/assets/js/detect.js",
                        "~/assets/js/fastclick.js",
                        "~/assets/js/jquery.blockUI.js",
                        "~/assets/js/waves.js",
                        "~/assets/js/wow.min.js", 
                        "~/assets/js/jquery.nicescroll.js",
                        "~/assets/js/jquery.scrollTo.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/assets/js/modernizr.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //            "~/bower_components/bootstrap/dist/js/bootstrap.js",
            //            "~/bower_components/bootstrap-modal/js/bootstrap-modalmanager.js",
            //            "~/bower_components/bootstrap-modal/js/bootstrap-modal.js",
            //            "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/assets/plugins/jquery-circliful/css/jquery.circliful.css",
                        "~/assets/css/bootstrap.min.css",
                        "~/assets/css/core.css",
                        "~/assets/css/components.css",
                        "~/assets/css/icons.css",
                        "~/assets/css/pages.css",
                        "~/assets/css/menu.css",
                        "~/assets/css/responsive.css"));

            
        }
    }
}
