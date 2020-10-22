using System.Web;
using System.Web.Optimization;

namespace eRegistrationCardSystem
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/jquery.dd.js",
                      "~/Scripts/Common.js",
                      "~/Scripts/datepicker.js",
                      "~/Scripts/datepicker.en.js",
                      "~/Scripts/SweetAlert2.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/html2canvas.js",                      
                      "~/Vendor/DataTables/DataTables-1.10.22/js/jquery.dataTables.min.js",
                      "~/Vendor/DataTables/DataTables-1.10.22/js/dataTables.bootstrap4.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/dd.css",
                      "~/Content/Global.css",
                      "~/Content/datepicker.css",
                      "~/Vendor/DataTables/DataTables-1.10.22/css/dataTables.bootstrap4.min.css"
                      ));
        }
    }
}
