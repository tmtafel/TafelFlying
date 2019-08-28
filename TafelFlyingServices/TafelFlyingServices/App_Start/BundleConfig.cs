using System.Web.Optimization;

namespace TafelFlyingServices
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.googlemaps.js",
                "~/Scripts/jquery.reversegeocode.js",
                "~/Scripts/jquery-ui.js",
                "~/Scripts/jquery-migrate-1.2.1.js",
                "~/Content/SimpleDropDownEffects/js/jquery.dropdown.js",
                "~/Content/Slick/Slick.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/fontawesome/font-awesome.css",
                "~/Content/SimpleDropDownEffects/css/common.css",
                "~/Content/SimpleDropDownEffects/css/icons.css",
                "~/Content/Slick/slick.css",
                "~/Content/button-themes.css",
                "~/Content/jquery-simplecolorpicker-master/jquery.simplecolorpicker.css",
                "~/Content/jquery-simplecolorpicker-master/jquery.simplecolorpicker-fontawesome.css",
                "~/Content/jquery-simplecolorpicker-master/jquery.simplecolorpicker-glyphicons.css",
                "~/Content/jquery-simplecolorpicker-master/jquery.simplecolorpicker-regularfont.css",
                "~/Content/jquery-ui.css"
                ));
            

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}