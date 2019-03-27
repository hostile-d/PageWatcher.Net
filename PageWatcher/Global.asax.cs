using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PageWatcher
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var serviceTask = new Tasks.ServiceTask();
            serviceTask.Start();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
