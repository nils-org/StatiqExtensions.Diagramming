using System.Xml.Linq;
using Diagramming.Tests.Helpers;
using Shouldly;
using Statiq.Extensions.Diagramming;
using Statiq.Testing;
using XTheory = Xunit.TheoryAttribute;

namespace Diagramming.Tests;

public class KrokiShortcodeTests
{
    [OnlyOnLocalKroki]
    public async Task Test1()
    {
        // Given
        var context = new TestExecutionContext();
        context.Settings.Add(SettingKeys.Kroki.ServiceUrl, LocalServices.Kroki);
        var document = new TestDocument();
        var shortcode = new KrokiShortcode();
        const string shortcodeContent = """
                                        digraph G {
                                          Hello->World
                                        }
                                        """;
        var shortcodeArgs = new[]
        {
            new KeyValuePair<string, string>("diagramType", "graphviz"),
            new KeyValuePair<string, string>("outputFormat", "svg"),
        };
        
        // When
        var result = await shortcode.ExecuteAsync(shortcodeArgs, shortcodeContent, document, context);
        
        // Then
        var content = await result.Single().ContentProvider.GetTextReader().ReadToEndAsync();
        var xml = XDocument.Parse(content);
        xml.Root!.Name.LocalName.ShouldBe("figure");
        var inner = xml.Root.Nodes().Single() as XElement;
        inner!.Name.LocalName.ShouldBe("svg");
    }

    [XTheory]
    [InlineData("svg")]
    [InlineData("png")]
    [InlineData("jpg")]
    public async Task Width_and_Height_get_set(string outputFormat)
    {
        var context = new TestExecutionContext()
            .WithKrokiServiceUrl()
            .WithMockWebAdapter();
        
        var document = new TestDocument();
        var shortcode = new KrokiShortcode();
        const string shortcodeContent = """
                                        digraph G {
                                          Hello->World
                                        }
                                        """;
        var shortcodeArgs = new[]
        {
            new KeyValuePair<string, string>("diagramType", "graphviz"),
            new KeyValuePair<string, string>("outputFormat", outputFormat),
            new KeyValuePair<string, string>("width", "20px"),
            new KeyValuePair<string, string>("height", "21px"),
            new KeyValuePair<string, string>("alt", "some alt text"),
        };
        
        // When
        var result = await shortcode.ExecuteAsync(shortcodeArgs, shortcodeContent, document, context);
        
        // Then
        var content = await result.Single().ContentProvider.GetTextReader().ReadToEndAsync();
        var svg = XDocument.Parse(content).Root!.Nodes().Single() as XElement;
        svg!.Attributes().Single(x => x.Name.LocalName == "width").Value.ShouldBe("20px");
        svg.Attributes().Single(x => x.Name.LocalName == "height").Value.ShouldBe("21px");
        if (outputFormat == "svg")
        {
            svg.Attributes().FirstOrDefault(x => x.Name.LocalName == "alt").ShouldBeNull();
        }
        else
        {
            svg.Attributes().Single(x => x.Name.LocalName == "alt").Value.ShouldBe("some alt text");
        }
    }
}