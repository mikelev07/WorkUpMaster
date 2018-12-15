using System.Web;
using System.Web.Optimization;

namespace HelpMe
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // готово к выпуску, используйте средство сборки по адресу https://modernizr.com, чтобы выбрать только необходимые тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/Scripts/Custom/js").Include(
                   "~/Scripts/Custom/jquery-migrate-3.0.0.min.js" ,
                   "~/Scripts/Custom/mmenu.min.js",
                   "~/Scripts/Custom/tippy.all.min.js",
                   "~/Scripts/Custom/bootstrap-slider.min.js",
                   "~/Scripts/Custom/bootstrap-select.min.js",
                   "~/Scripts/Custom/snackbar.js" ,
                   "~/Scripts/Custom/clipboard.min.js",
                   "~/Scripts/Custom/counterup.min.js",
                   "~/Scripts/Custom/magnific-popup.min.js",
                   "~/Scripts/Custom/slick.min.js",
                   "~/Scripts/Custom/custom.js"
           ));

        
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Custom/css").Include(
                "~/Content/Custom/style.css",
                "~/Content/Custom/colors/green.css").Include("~/Content/Custom/icons.css", new CssRewriteUrlTransform()));

        }
    }
}
