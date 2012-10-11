
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using System.Web.Routing;

namespace XRouter
{
  public class XRouteMonitor: IRouteMonitor, IDisposable
  {
    private FileSystemWatcher _watcher;
    private readonly string _routeConfigFileName;

    public XRouteMonitor(string routeConfigFileName)
    {
      if (string.IsNullOrEmpty(routeConfigFileName))
        throw new ArgumentException("The routeConfigFileName should not be null or empty.");

      _routeConfigFileName = routeConfigFileName;
    }

    public void Monitor(IRouteSource routeSource, Action<RouteCollection, IEnumerable<RouteItem>> action)
    {
      _watcher = new FileSystemWatcher(HostingEnvironment.MapPath("~/"), _routeConfigFileName);
      _watcher.IncludeSubdirectories = true;
      _watcher.EnableRaisingEvents = true;
      _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
      _watcher.Changed += delegate(object sender, FileSystemEventArgs e)
      {
        _watcher.EnableRaisingEvents = false;
        action(RouteTable.Routes, routeSource.GetRouteItems());
        _watcher.EnableRaisingEvents = true;
      };
    }

    public void Dispose()
    {
      if (_watcher != null)
        _watcher.Dispose();
    }
  }
}