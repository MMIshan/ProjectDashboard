using System.Web;
using System.Web.Optimization;

namespace ProDashBoard
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                       "~/Scripts/angular.min.js",
                       "~/Scripts/higgidy_carousel.js",
                       "~/Scripts/higgidy_carousel-Copy.js",
                       "~/Scripts/ui-router.min.js",
                       "~/Scripts/Chart.min.js",
                       "~/Scripts/angular-chart.min.js",
                       "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                        "~/Scripts/angular-animate.min.js",
                        "~/Scripts/angular-aria.min.js",
                        "~/Scripts/angular-messages.min.js",
                       "~/Scripts/angular-material/angular-material.min.js",
                       "~/Scripts/toaster.min.js",
                       "~/Scripts/angular-spinner.min.js",
                       "~/Scripts/pie-chart.min.js",
                       "~/Scripts/bootstrap-select.min.js",
                       "~/Scripts/angular-sanitize.js"
                       ));


            


            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-select.min.css",
                      "~/Content/angular-chart.min.css", 
                      "~/Content/site.css",
                      "~/Content/font-awesome.css",
                      "~/Content/angular-material.css",
                      "~/Content/toaster.css",
                      "~/Content/higgidy_carousel.css",
                      "~/Content/higgidy_carousel-Copy.css",
                      "~/Content/pie-chart.css"));

            bundles.Add(new ScriptBundle("~/bundles/appjs").Include(
                     "~/app/app.js",
                    "~/app/teamSatisfaction/teamCtrl.js",
                    "~/app/home/projectCtrl.js",
                    "~/app/teamSatisfaction/popupCtrl.js",
                    "~/app/teamSatisfaction/teamFormCtrl.js",
                    "~/app/customerSatisfaction/customerSatisfactionCtrl.js",
                    "~/app/processCompliance/processComplianceCtrl.js",
                    "~/app/processCompliance/processComplianceFormCtrl.js",
                    "~/app/error/errorCtrl.js",
                    "~/app/adminPanel/adminPanelCtrl.js",
                    "~/app/adminPanel/adminÀccountCtrl.js",
                    "~/app/adminPanel/adminProjectCtrl.js",
                    "~/app/adminPanel/adminEmployeesCtrl.js",
                    "~/app/financialStuff/financialFormCtrl.js",
                    "~/app/financialStuff/financialStatusCtrl.js"
                    ));

        }
    }
}
