using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NewFront2.Actors;
using NewFront2.Providers;
using TaxiShared;

namespace NewFront2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            

            RegisterActors();
            RegisterBundles();

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private static void RegisterBundles()
        {
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/js")
                .IncludeDirectory("~/Scripts", "*.js")
                .IncludeDirectory("~/App", "*.js"));
        }

        private static void RegisterActors()
        {
            ActorSystem.Start();
        }
    }
}
