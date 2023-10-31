namespace Diagramming.Tests.Helpers;

internal static class LocalServices
{
    public static string Kroki
    {
        get
        {
            var url = Environment.GetEnvironmentVariable("KROKI_SERVER");
            return url ?? "http://localhost:8000/";
        }
    }
}