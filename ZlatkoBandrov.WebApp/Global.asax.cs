using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZlatkoBandrov.BusinessLogic.Managers;

namespace ZlatkoBandrov.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static bool isInitializedArrivalTracking = false;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            // Initialize the communication between the two Web Api services
            if (!isInitializedArrivalTracking)
            {
                var trackingManager = new ArrivalTrackerManager();
                trackingManager.InitializeCommunication();
                isInitializedArrivalTracking = true;
            }
        }
    }
}
