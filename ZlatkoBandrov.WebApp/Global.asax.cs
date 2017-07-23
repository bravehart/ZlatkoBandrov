using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZlatkoBandrov.BusinessLogic.Managers;

namespace ZlatkoBandrov.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Initialize the communication between the two Web Api services
            var trackingManager = new ArrivalTrackerManager();
            trackingManager.InitializeCommunication();
        }
    }
}
