using System.Web.Optimization;

namespace InfoPublishSystem
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // 使用普通 Bundle 并清除变换，避免调用默认的 ScriptBundle 的 JsMinify（会使用 Microsoft Ajax Minifier）
            var bootstrapBundle = new Bundle("~/bundles/bootstrap");
            bootstrapBundle.Include("~/Scripts/bootstrap.bundle.min.js");
            bootstrapBundle.Transforms.Clear(); // 确保不再对包含的文件进行再次压缩/解析
            bundles.Add(bootstrapBundle);

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css"));
        }
    }
}
