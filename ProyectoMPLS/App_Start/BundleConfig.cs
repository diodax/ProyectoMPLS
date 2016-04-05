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
                        "~/bower_components/jquery/dist/jquery.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/bower_components/bootstrap/dist/js/bootstrap.js",
                        "~/bower_components/bootstrap-modal/js/bootstrap-modalmanager.js",
                        "~/bower_components/bootstrap-modal/js/bootstrap-modal.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/bower_components/bootstrap/dist/css/bootstrap.css",
                        "~/bower_components/bootstrap-modal/css/bootstrap-modal-bs3patch.css",
                        "~/bower_components/bootstrap-modal/css/bootstrap-modal.css",
                        "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/modalform").Include(
                        "~/Scripts/modalform.js"));
        }
    }
}
