
using System.Collections.Generic;

namespace XRouter
{
  /// <summary>
  /// Contains the details of an Area.
  /// </summary>
  public class Area
  {
    /// <summary>
    /// Name of the area.
    /// </summary>
    public string Name
    { get; set; }

    /// <summary>
    /// Namespaces of the controllers.
    /// </summary>
    public string[] Namespaces
    { get; set; }
  }
}
