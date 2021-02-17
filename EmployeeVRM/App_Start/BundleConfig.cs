using System.Web;
using System.Web.Optimization;

namespace EmployeeVRM
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.js",
                         "~/Scripts/moment.js"
                         ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/bootstrap.bundle.min.js",
                     "~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/toastr.min.js"
                     ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                            "~/Content/bootstrap.css",
                            "~/Content/UIContent/plugins/fontawesome-free/css/all.min.css",
                           "~/Content/UIContent/css/adminlte.min.css",
                           "~/Content/bootstrap-datetimepicker.css",
                        "~/Content/toastr.min.css",
                       "~/Content/Site.css"
                      
                      ));

            bundles.Add(new ScriptBundle("~/adminlte/js").Include(
                         "~/Content/UIContent/js/adminlte.min.js"));
        }
    }
}
