
using System.Web.Routing;

namespace XRouter.Tests
{
  public class SampleConstraint: IRouteConstraint
  {
    public bool Match(System.Web.HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
    {
      throw new System.NotImplementedException();
    }
  }
}
