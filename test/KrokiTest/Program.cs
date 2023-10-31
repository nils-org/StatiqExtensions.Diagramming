using Statiq.Extensions.Diagramming;

return await Bootstrapper
    .Factory
    .CreateWeb(args)
    .AddSetting(SettingKeys.Kroki.ServiceUrl, GetKrokiServiceUrl())
    .AddShortcode<KrokiShortcode>()
    .RunAsync();

string GetKrokiServiceUrl()
{
    var url = Environment.GetEnvironmentVariable("KROKI_URL");
    return string.IsNullOrEmpty(url) ? "http://localhost:8000/" : url;
}