
using System.Web.Routing;

namespace TrafficPolice
{
  public class IgnoreRoute : Route
  {
    public IgnoreRoute(string url)
      : base(url, new StopRoutingHandler())
    {
    }

    public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary routeValues)
    {
      return null;
    }
  }
}