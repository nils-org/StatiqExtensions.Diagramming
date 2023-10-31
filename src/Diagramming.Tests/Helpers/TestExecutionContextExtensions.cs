using Statiq.Extensions.Diagramming;
using Statiq.Testing;

namespace Diagramming.Tests.Helpers;

public static class TestExecutionContextExtensions
{
    public static TestExecutionContext WithKrokiServiceUrl(this TestExecutionContext context)
    {
        context.Settings.Add(SettingKeys.Kroki.ServiceUrl, "http://localhost/");
        return context;
    }
    
    public static TestExecutionContext WithMockWebAdapter(this TestExecutionContext context)
    {
        context.Settings.Add(SettingKeys.WebAdapter, new MockWebAdapter());
        return context;
    }
    
}