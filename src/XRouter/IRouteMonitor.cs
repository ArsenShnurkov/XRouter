
using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace XRouter
{
  /// <summary>
  /// Represents the component that monitors the route source for changes.
  /// </summary>
  public interface IRouteMonitor
  {
    void Monitor(IRouteSource routeSource, Action<RouteCollection, IEnumerable<RouteItem>> action);
  }
}
