
using Microsoft.Extensions.Logging;

namespace Statiq.Extensions.Diagramming.Web;

/// <summary>
/// An adapter to make web-requests.
/// If you can't use the default implementation,
/// set a new instance of this interface using the
/// <see cref="SettingKeys.WebAdapter"/> settings key.
/// </summary>
public interface IWebAdapter
{
    /// <summary>
    /// Send a POST request.
    /// </summary>
    /// <param name="url">The Url to send to.</param>
    /// <param name="content">The content to send.</param>
    /// <param name="headers">Additional headers to set on the request.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The response content.</returns>
    Task<HttpContent> Post(string url, string content, Dictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
}

internal class DefaultWebAdapter : IWebAdapter
{
    private readonly ILogger _logger;

    public DefaultWebAdapter(ILogger logger)
    {
        _logger = logger;
    }
    
    public async Task<HttpContent> Post(
        string url, 
        string? content, 
        Dictionary<string, string>? headers = null, 
        CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();

        HttpContent? httpContent = null;
        if (content != null)
        {
            httpContent = new StringContent(content);
        }

        if (headers != null)
        {
            foreach (var header in headers)
            { 
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        var responseMessage = await client.PostAsync(new Uri(url), httpContent, cancellationToken);
        if (!responseMessage.IsSuccessStatusCode)
        {
            _logger.LogError($"{responseMessage.StatusCode}-Error sending POST: {responseMessage.ReasonPhrase ?? "Error"}");
        }

        responseMessage.EnsureSuccessStatusCode();
        return responseMessage.Content;
    }
}