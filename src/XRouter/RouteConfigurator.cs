
#region usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
#endregion

namespace XRouter
{
  /// <summary>
  /// The main class that configures the routes and monitors for changes in routes.
  /// </summary>
  public static class RouteConfigurator
  {
    private static string _routeConfigFileName = "routes.config";

    public static string RouteConfigFileName
    {
      get
      {
        return _routeConfigFileName;
      }
      set
      {
        _routeConfigFileName = value;
      }
    }

    public static void Configure()
    {
      var routeSource = new XRouteSource(RouteConfigFileName);

      Register(RouteTable.Routes, routeSource.GetRouteItems());

      new XRouteMonitor(RouteConfigFileName).Monitor(routeSource, Register);
    }

    public static void Configure(IRouteSource routeSource, IRouteMonitor routeMonitor = null)
    {
      if (routeSource == null)
        throw new ArgumentNullException("routeSource");

      Register(RouteTable.Routes, routeSource.GetRouteItems());

      if (routeMonitor != null)
        routeMonitor.Monitor(routeSource, Register);
    }

    private static void Register(RouteCollection routes, IEnumerable<RouteItem> routeItems)
    {
      using (var @lock = routes.GetWriteLock())
      {
        routes.Clear();

        foreach (var routeItem in routeItems.Where(r => !r.Disabled))
        {
          routes.Add(routeItem.Name, new RouteAdapter(routeItem));
        }
      }
    }
  }
}