using System.Xml.Linq;
using JetBrains.Annotations;
using Statiq.Common;
using Statiq.Extensions.Diagramming.Web;

namespace Statiq.Extensions.Diagramming;

/// <summary>
/// The Shortcode to create a diagram using <see href="https://kroki.io"/>.
/// </summary>
[UsedImplicitly]
public class KrokiShortcode : IShortcode
{
    private const string DiagramType = nameof(DiagramType);
    private const string OutputFormat = nameof(OutputFormat);
    private const string Alt = nameof(Alt);
    private const string Caption = nameof(Caption);
    private const string Height = nameof(Height);
    private const string Width = nameof(Width);
    
    /// <inheritdoc cref="IShortcode"/>
    public async Task<IEnumerable<ShortcodeResult>> ExecuteAsync(
        KeyValuePair<string, string>[] args,
        string inputContent,
        IDocument document, IExecutionContext context)
    {
        if (!context.Settings.ContainsKey(SettingKeys.Kroki.ServiceUrl))
        {
            const string message = $"The {nameof(SettingKeys.Kroki.ServiceUrl)} setting key must be set!";
            context.LogCritical(document, message);
            throw new ArgumentException(message);
        }
        
        var arguments = args.ToDictionary(
            Alt,
            Caption,
            Height,
            Width);
        
        var krokiServiceUrl = context.Settings.Get<string>(SettingKeys.Kroki.ServiceUrl);
        var diagramType = args.FirstOrDefault(x => x.Key.Equals(DiagramType, StringComparison.OrdinalIgnoreCase)).Value;
        var outputFormat = args.FirstOrDefault(x => x.Key.Equals(OutputFormat, StringComparison.OrdinalIgnoreCase)).Value;
        
        if (string.IsNullOrWhiteSpace(diagramType))
        {
            const string message = "The argument 'diagramType' must be set!";
            context.LogCritical(document, message);
            throw new ArgumentException(message);
        }
        if (string.IsNullOrWhiteSpace(outputFormat))
        {
            const string message = "The argument 'outputFormat' must be set!";
            context.LogCritical(document, message);
            throw new ArgumentException(message);
        }
        if (string.IsNullOrWhiteSpace(inputContent))
        {
            const string message = "The Shortcode has no content. No diagram will be created!";
            context.LogCritical(document, message);
            throw new ArgumentException(message);
        }
        
        var figure = new XElement("figure");
        var caption = arguments.XElement(Caption, x => new XElement("figcaption", x));
        if (caption != null)
        {
            figure.Add(caption);
        }
        
        outputFormat = outputFormat.ToLowerInvariant();
        if (outputFormat.Equals("jpg", StringComparison.OrdinalIgnoreCase))
        {
            outputFormat = "jpeg";
        }
        var allowedOutputFormats = new[]
        {
            "svg",
            "png",
            "jpeg"
        };
        if(!allowedOutputFormats.Contains(outputFormat))
        {
            outputFormat = "svg";
            context.LogError(document, $"The selected output format, '{outputFormat}' is not allowed. Valid values are {string.Join(", ", allowedOutputFormats.OrderBy(x => x))}. SVG is used as a fallback.");
        }

        var webAdapter = context.Settings.Get<IWebAdapter>(SettingKeys.WebAdapter, new DefaultWebAdapter(context));
        var url = $"{krokiServiceUrl.TrimEnd('/')}/{diagramType}/{outputFormat}";
        context.LogTrace(document, $"sending Kroki POST to url '{url}', Content:\n{inputContent}");
        var outputContent = await webAdapter.Post(
            url,
            inputContent
        );

        if (outputFormat == "svg")
        {
            var svgContent = await outputContent.ReadAsStringAsync();
            var svgElem = XDocument.Parse(svgContent).Root!;

            foreach (var name in new[]{Width, Height})
            {
                var attr = arguments.XAttribute(name);
                if (attr == null)
                {
                    continue;
                }
                
                svgElem.Attributes()
                    .FirstOrDefault(x => x.Name.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase))?
                    .Remove();
                svgElem.Add(attr);
            }
            
            figure.Add(svgElem);
        }
        else
        {
            var binaryContent = await outputContent.ReadAsByteArrayAsync();
            var base64Content = Convert.ToBase64String(binaryContent);
            var img = new XElement("img");
            img.Add(new XAttribute("src", $"data:image/{outputFormat};base64,{base64Content}"));
            img.Add(arguments.XAttribute(Alt));
            img.Add(arguments.XAttribute(Width));
            img.Add(arguments.XAttribute(Height));
            figure.Add(img);
        }

        return new ShortcodeResult[]
        {
            figure.ToString()   
        };
    }
}