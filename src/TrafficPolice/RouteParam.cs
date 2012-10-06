
namespace TrafficPolice
{
  /// <summary>
  /// Contains all the details of the parameters in url pattern other than controller & action.
  /// </summary>
  public class RouteParam
  {
    /// <summary>
    /// The name of the route parameter.
    /// </summary>
    public string Name
    { get; set; }

    /// <summary>
    /// The default value of the route parameter.
    /// </summary>
    public string Value
    { get; set; }

    /// <summary>
    /// The data type(int or date) of the parameter, actually it's a constraint.
    /// </summary>
    public string Type
    { get; set; }

    /// <summary>
    /// Whether the parameter is optional or not.
    /// </summary>
    public bool Optional
    { get; set; }

    /// <summary>
    /// Regular expression constraint.
    /// </summary>
    public string Constraint
    { get; set; }
  }
}