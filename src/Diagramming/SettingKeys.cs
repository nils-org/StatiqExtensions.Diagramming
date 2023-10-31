using Statiq.Extensions.Diagramming.Web;

namespace Statiq.Extensions.Diagramming;

/// <summary>
/// Settings keys.
/// </summary>
public static class SettingKeys
{
    /// <summary>
    /// An Adapter to access the web. Use it only,
    /// if you really require to control the access to web yourself.
    /// Set it to an instance of <see cref="IWebAdapter"/>.
    /// </summary>
    public static string WebAdapter = nameof(Diagramming) + nameof(WebAdapter);
    
    /// <summary>
    /// All the setting keys used for Diagramming
    /// </summary>
    public static class Kroki
    {
        /// <summary>
        /// The Url of the Diagramming service to use.
        /// If you dont have a custom hosted Diagramming instance, use "https://kroki.io".
        /// However, if you make heavy use of the service either host your own
        /// instance or get in contact with the people behind Kroki.
        /// <seealso href="https://kroki.io/#install"/>
        /// </summary>
        public static string ServiceUrl = nameof(Diagramming) + nameof(ServiceUrl);
    }
}
