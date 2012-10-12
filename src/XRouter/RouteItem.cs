
using System.Collections.Generic;

namespace XRouter
{
  /// <summary>
  /// Represents the details for a route. The route may be for map, ignore or redirect a request.
  /// </summary>
  public class RouteItem
  {
    /// <summary>
    /// The Name of a route. This is required if you have setup a constraint at the route level 
    /// (HttpMethods or Constraint)
    /// </summary>
    public string Name
    { get; set; }

    /// <summary>
    /// The url pattern ex. {controller}/{action}/{id}
    /// </summary>
    public string Pattern
    { get; set; }

    /// <summary>
    /// Contains the details of the area. Would be null if the route not belongs to any area.
    /// </summary>
    public Area Area
    { get; set; }

    /// <summary>
    /// Name of the default controller.
    /// </summary>
    public string Controller
    { get; set; }

    /// <summary>
    /// Name of the default action.
    /// </summary>
    public string Action
    { get; set; }

    /// <summary>
    /// True to ignore the requests if matches the pattern.
    /// </summary>
    public bool Ignore
    { get; set; }

    /// <summary>
    /// True to redirect the request if matches the pattern to the one specified in the RedirectParams.
    /// </summary>
    public bool Redirect
    { get; set; }

    /// <summary>
    /// Contains the details of the redirection.
    /// </summary>
    public RedirectParams RedirectParams
    { get; set; }

    /// <summary>
    /// True to handle the request even there is a matching file exists.
    /// </summary>
    public bool RouteExistingFiles
    { get; set; }

    /// <summary>
    /// The http methods allowed to handle by the route.
    /// </summary>
    public string[] HttpMethods
    { get; set; }

    /// <summary>
    /// Type that implements IRouteConstraint.
    /// </summary>
    public string Constraint
    { get; set; }

    /// <summary>
    /// The collection of route parameters.
    /// </summary>
    public RouteParam[] Params
    { get; set; }

    /// <summary>
    /// Collection of constraints.
    /// </summary>
    public RouteConstraint[] Constraints
    { get; set; }

    /// <summary>
    /// Datatokens.
    /// </summary>
    public IDictionary<string, object> DataTokens
    { get; set; }

    /// <summary>
    /// Controller namespaces.
    /// </summary>
    public string[] Namespaces
    { get; set; }

    /// <summary>
    /// True if the route is disabled.
    /// </summary>
    public bool Disabled
    { get; set; }
  }
}