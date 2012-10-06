
using System.Collections.Generic;

namespace TrafficPolice
{
  /// <summary>
  /// Represents a source(xml, database) where the routes should be read from.
  /// </summary>
  public interface IRouteSource
  {
    /// <summary>
    /// Returns collection of RouteItems from the source.
    /// </summary>
    /// <returns></returns>
    List<RouteItem> GetRouteItems();
  }
}
