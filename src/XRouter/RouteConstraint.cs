
namespace XRouter
{
  /// <summary>
  /// Contains the contraints details of a route.
  /// </summary>
  public class RouteConstraint
  {
    /// <summary>
    /// Name of the constraint, mandatory.
    /// </summary>
    public string Name
    { get; set; }

    /// <summary>
    /// The full name of the type that implements IRouteConstraint.
    /// </summary>
    public string Value
    { get; set; }

    /// <summary>
    /// Whether the constraint is disabled or not.
    /// </summary>
    public bool Disabled
    { get; set; }
  }
}
