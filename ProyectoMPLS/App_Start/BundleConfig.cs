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

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                        "~/assets/plugins/datatables/jquery.dataTables.min.js",
                        "~/assets/plugins/datatables/dataTables.bootstrap.js",
                        "~/assets/plugins/datatables/dataTables.buttons.min.js",
                        "~/assets/plugins/datatables/buttons.bootstrap.min.js",
                        "~/assets/plugins/datatables/jszip.min.js",
                        "~/assets/plugins/datatables/pdfmake.min.js",
                        "~/assets/plugins/datatables/vfs_fonts.js",
                        "~/assets/plugins/datatables/buttons.html5.min.js",
                        "~/assets/plugins/datatables/buttons.print.min.js",
                        "~/assets/plugins/datatables/dataTables.fixedHeader.min.js",
                        "~/assets/plugins/datatables/dataTables.keyTable.min.js",
                        "~/assets/plugins/datatables/dataTables.responsive.min.js",
                        "~/assets/plugins/datatables/responsive.bootstrap.min.js",
                        "~/assets/plugins/datatables/dataTables.scroller.min.js"));

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
