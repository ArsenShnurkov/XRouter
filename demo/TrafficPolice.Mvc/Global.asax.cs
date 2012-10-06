using System.Web.Mvc;

namespace TrafficPolice.Mvc
{
  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfigurator.Configure();  
    }
  }
}