
using System.Web;
using System.Web.Routing;

namespace TrafficPolice
{
  public class RedirectRoute : Route
  {
    public RedirectRoute(string pattern, RedirectParams redirectParams)
      : base(pattern, new RedirectRouteHandler(redirectParams))
    {
    }
  }

  public class RedirectRouteHandler : IRouteHandler
  {
    private readonly RedirectParams _redirectParams;

    public RedirectRouteHandler(RedirectParams redirectParams)
    {
      _redirectParams = redirectParams;
    }

    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      if (_redirectParams.RedirectTo.StartsWith("~/"))
      {
        string virtualPath = _redirectParams.RedirectTo.Substring(2);
        Route route = new Route(virtualPath, null);
        var vpd = route.GetVirtualPath(requestContext, requestContext.RouteData.Values);
        if (vpd != null)
        {
          _redirectParams.RedirectTo = "~/" + vpd.VirtualPath;
        }
      }

      return new RedirectHandler(_redirectParams, false);
    }
  }

  public class RedirectHandler : IHttpHandler
  {
    private readonly bool _isReusable;
    private readonly RedirectParams _redirectParams;

    public RedirectHandler(RedirectParams redirectParams, bool isReusable)
    {
      _redirectParams = redirectParams;
      _isReusable = isReusable;
    }

    public bool IsReusable
    {
      get { return _isReusable; }
    }

    public void ProcessRequest(HttpContext context)
    {
      if (_redirectParams.IsPermanent)
      {
        context.Response.Status = "301 Moved Permanently";
        context.Response.StatusCode = 301;
        context.Response.AddHeader("Location", _redirectParams.RedirectTo);
      }
      else
      {
        context.Response.Redirect(_redirectParams.RedirectTo, false);
      }
    }
  }
}