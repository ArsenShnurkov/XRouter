
namespace XRouter
{
  /// <summary>
  /// Contains the redirection details.
  /// </summary>
  public class RedirectParams
  {
    /// <summary>
    /// The url to redirect.
    /// </summary>
    public string RedirectTo
    { get; set; }

    /// <summary>
    /// Redirect permanently or not.
    /// </summary>
    public bool IsPermanent
    { get; set; }
  }
}
