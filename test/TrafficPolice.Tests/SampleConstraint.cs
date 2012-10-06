
using System.Web.Routing;

namespace TrafficPolice.Tests
{
  public class SampleConstraint: IRouteConstraint
  {
    public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
    {
      throw new System.NotImplementedException();
    }
  }
}
