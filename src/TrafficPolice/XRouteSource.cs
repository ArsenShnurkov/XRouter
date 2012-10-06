
#region usings
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;
#endregion

namespace TrafficPolice
{
  /// <summary>
  /// This class reads the routes defined in an xml file and returs them as a collection of RouteItems.
  /// </summary>
  public class XRouteSource : IRouteSource
  {
    private readonly string _routeConfigFileName;

    // for unit testing
    private string _directoryPath;

    public string DirectoryPath
    {
      get
      {
        if (String.IsNullOrEmpty(_directoryPath))
          return HostingEnvironment.MapPath("~/");

        return _directoryPath;
      }
      set
      {
        _directoryPath = value;
      }
    }

    public XRouteSource(string routeConfigFileName)
    {
      if (string.IsNullOrEmpty(routeConfigFileName))
        throw new ArgumentException("The routeConfigFileName should not be null or empty.");

      _routeConfigFileName = routeConfigFileName;
    }

    public List<RouteItem> GetRouteItems()
    {
      var routeItems = new List<RouteItem>();

      AddAreaRouteItems(routeItems);

      AddDefaultRouteItems(routeItems);

      return routeItems;
    }

    private void AddAreaRouteItems(List<RouteItem> routeItems)
    {
      if (Directory.Exists(Path.Combine(DirectoryPath, "Areas")))
      {
        foreach (var routeConfigFile in Directory.GetFiles(Path.Combine(DirectoryPath, "Areas"), _routeConfigFileName, SearchOption.AllDirectories))
        {
          var areaConfigElement = XElement.Load(routeConfigFile);
          routeItems.AddRange(GetRouteItems(areaConfigElement, true));
        }
      }
    }

    private void AddDefaultRouteItems(List<RouteItem> routeItems)
    {
      if (File.Exists(Path.Combine(DirectoryPath, _routeConfigFileName)))
      {
        var rootConfigElement = XElement.Load(Path.Combine(DirectoryPath, _routeConfigFileName));

        if (rootConfigElement.Name.ToString().Equals("areas"))
        {
          foreach (var area in rootConfigElement.Elements("area"))
          {
            routeItems.AddRange(GetRouteItems(area, true));
          }
        }
        else
        {
          routeItems.AddRange(GetRouteItems(rootConfigElement));
        }
      }
    }

    private List<RouteItem> GetRouteItems(XElement el, bool isArea = false)
    {
      var routeItems = new List<RouteItem>();

      if (el == null) return routeItems;

      XElement routes = isArea ? el.Element("routes") : el;

      foreach (var e in routes.Elements().Where(e => e.Name.ToString().Equals("map") || e.Name.ToString().Equals("ignore") || e.Name.ToString().Equals("redirect")))
      {
        var routeItem = new RouteItem
        {
          Name = GetAttributeVal(e, "name"),

          Pattern = GetAttributeVal(e, "pattern"),

          Area = isArea
            ? new Area
            {
              Name = GetAttributeVal(el, "name"),
              Namespaces = el.Element("namespaces") != null ? el.Element("namespaces").Elements("ns").Select(t => t.Value).ToArray() : new string[] { }
            }
            : null,

          Disabled = !String.IsNullOrEmpty(GetAttributeVal(e, "disabled"))
            ? bool.Parse(GetAttributeVal(e, "disabled"))
            : false,

          Ignore = e.Name.ToString().Equals("ignore"),

          Redirect = e.Name.ToString().Equals("redirect")
        };

        if (routeItem.Redirect)
        {
          routeItem.RedirectParams = new RedirectParams
          {
            RedirectTo = GetAttributeVal(e, "to"),
            IsPermanent = !String.IsNullOrEmpty(GetAttributeVal(e, "permanent")) ? bool.Parse(GetAttributeVal(e, "permanent")) : true
          };
        }
        else
        {
          routeItem.HttpMethods = !String.IsNullOrEmpty(GetAttributeVal(e, "method"))
            ? GetAttributeVal(e, "method").Split(',')
            : null;

          routeItem.Constraint = GetAttributeVal(e, "constraint");

          routeItem.Constraints = e.Element("constraints") != null
            ? e.Element("constraints").Elements("constraint").Select(c => new RouteConstraint
              {
                Name = GetAttributeVal(c, "name"),
                Value = GetAttributeVal(c, "value"),
                Disabled = !String.IsNullOrEmpty(GetAttributeVal(c, "disabled"))
                  ? bool.Parse(GetAttributeVal(c, "disabled"))
                  : false,
              }).ToArray()
            : null;
        }

        if (!routeItem.Ignore && !routeItem.Redirect)
        {
          routeItem.Controller = GetAttributeVal(e, "controller");

          routeItem.Action = GetAttributeVal(e, "action");

          routeItem.RouteExistingFiles = !String.IsNullOrEmpty(GetAttributeVal(e, "routefiles"))
            ? bool.Parse(GetAttributeVal(e, "routefiles"))
            : false;

          routeItem.Params = e.Element("params") != null
            ? e.Element("params").Elements("param").Select(p => new RouteParam
              {
                Name = GetAttributeVal(p, "name"),
                Value = GetAttributeVal(p, "value"),
                Type = GetAttributeVal(p, "type"),
                Optional = !String.IsNullOrEmpty(GetAttributeVal(p, "optional"))
                  ? bool.Parse(GetAttributeVal(p, "optional"))
                  : false,
                Constraint = GetAttributeVal(p, "constraint")
              }).ToArray()
            : null;

          routeItem.DataTokens = e.Element("tokens") != null
            ? e.Element("tokens").Elements("token").ToDictionary(i => GetAttributeVal(i, "name"), i => (object)GetAttributeVal(i, "value"))
            : null;

          routeItem.Namespaces = e.Element("namespaces") != null
            ? e.Element("namespaces").Elements("ns").Select(t => t.Value).ToArray()
            : null;
        }

        if (isArea)
        {
          var areaNs = GetAttributeVal(el, "ns");

          if (!string.IsNullOrEmpty(areaNs))
          {
            if (routeItem.Namespaces == null)
              routeItem.Namespaces = new[] { String.Concat(areaNs, ".*") };
            else
              routeItem.Namespaces[routeItem.Namespaces.Length] = String.Concat(areaNs, ".*");
          }
        }

        routeItems.Add(routeItem);
      }

      return routeItems;
    }

    private static string GetAttributeVal(XElement el, string attribute)
    {
      return el.Attribute(attribute) != null ? el.Attribute(attribute).Value : "";
    }
  }
}