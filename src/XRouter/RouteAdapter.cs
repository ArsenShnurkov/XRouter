
#region usings
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
#endregion

namespace XRouter
{
  /// <summary>
  /// This is an adapter used to convert RouteItem into a Route.
  /// </summary>
  public class RouteAdapter : Route
  {
    private readonly RouteItem _routeItem;
    private readonly Route _route;

    public Route Route
    {
      get { return _route; }
    }

    public RouteAdapter(RouteItem routeItem)
      : base(routeItem.Pattern, new MvcRouteHandler())
    {
      if (routeItem == null) throw new ArgumentNullException("routeItem");

      _routeItem = routeItem;

      if (_routeItem.Ignore)
      {
        _route = new IgnoreRoute(routeItem.Pattern) { Constraints = GetConstraints(routeItem) };
      }
      else if (_routeItem.Redirect)
      {
        _route = new RedirectRoute(routeItem.Pattern, _routeItem.RedirectParams);
      }
      else
      {
        var route = new Route(routeItem.Pattern, new MvcRouteHandler())
        {
          Defaults = GetDefaults(routeItem),
          Constraints = GetConstraints(routeItem),
          DataTokens = routeItem.DataTokens != null ? new RouteValueDictionary(routeItem.DataTokens) : new RouteValueDictionary(),     
          RouteExistingFiles = routeItem.RouteExistingFiles
        };

        if (routeItem.Namespaces != null && routeItem.Namespaces.Length > 0)
          route.DataTokens["Namespaces"] = routeItem.Namespaces;
        else if (routeItem.Area != null && routeItem.Area.Namespaces != null && routeItem.Area.Namespaces.Length > 0)
          route.DataTokens["Namespaces"] = routeItem.Area.Namespaces;

        if (routeItem.Area != null)
        {
          route.DataTokens["area"] = routeItem.Area.Name;

          var namespaces = (routeItem.Namespaces == null && routeItem.Area.Namespaces != null)
            ? routeItem.Area.Namespaces
            : routeItem.Namespaces;

          route.DataTokens["UseNamespaceFallback"] = (namespaces == null || namespaces.Length == 0);
        }

        _route = route;

        Url = _route.Url;
        DataTokens = _route.DataTokens;
        Constraints = _route.Constraints;
        Defaults = _route.Defaults;
      }
    }

    public override RouteData GetRouteData(HttpContextBase httpContext)
    {
      return _route.GetRouteData(httpContext);
    }

    public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
    {
      return _route.GetVirtualPath(requestContext, values);
    }

    private RouteValueDictionary GetDefaults(RouteItem routeItem)
    {
      var defaults = new RouteValueDictionary();

      if (!string.IsNullOrEmpty(routeItem.Controller))
        defaults.Add("controller", routeItem.Controller);

      if (!string.IsNullOrEmpty(routeItem.Action))
        defaults.Add("action", routeItem.Action);

      if (routeItem.Params != null)
      {
        foreach (var routeParam in routeItem.Params)
        {
          defaults.Add(routeParam.Name, routeParam.Optional ? (object)UrlParameter.Optional : (object)routeParam.Value);
        }
      }

      return defaults;
    }

    private RouteValueDictionary GetConstraints(RouteItem routeItem)
    {
      var constraints = new RouteValueDictionary();

      if (routeItem.HttpMethods != null && routeItem.HttpMethods.Length > 0)
        constraints.Add("httpMethod", new HttpMethodConstraint(routeItem.HttpMethods));

      if (!String.IsNullOrEmpty(routeItem.Constraint))
        constraints.Add("custom", CreateConstraintInstance(routeItem.Constraint));

      if (routeItem.Constraints != null)
      {
        foreach (var constraint in routeItem.Constraints.Where(p => !p.Disabled))
        {
          constraints.Add(constraint.Name, CreateConstraintInstance(constraint.Value));
        }
      }

      if (routeItem.Params != null)
      {
        foreach (var routeParam in routeItem.Params)
        {
          if (!String.IsNullOrEmpty(routeParam.Type))
          {
            if (routeParam.Type == "int")
              constraints.Add(routeParam.Name, @"^\d+$");
            else if (routeParam.Type == "date")
              constraints.Add(routeParam.Name, @"\d{4}-\d{2}-\d{2}");
          }

          if (!String.IsNullOrEmpty(routeParam.Constraint))
            constraints.Add(routeParam.Name, routeParam.Constraint);
        }
      }

      return constraints;
    }

    private object CreateConstraintInstance(string constraint)
    {
      if (constraint.Contains(','))
      {
        var assembly = Assembly.LoadWithPartialName(constraint.Split(',')[1].Trim());

        if (assembly != null)
          return assembly.GetType(constraint.Split(',')[0].Trim());

        throw new Exception(String.Format("Assembly {0} not found", constraint.Split(',')[1].Trim()));
      }

      return Activator.CreateInstance(Type.GetType(constraint, true));
    }
  }
}