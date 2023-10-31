namespace Diagramming.Tests.Helpers;

internal class OnlyOnLocalKrokiAttribute : FactAttribute
{
    public override string Skip
    {
        get
        {
            try
            {
                var client = new HttpClient();
                var resp = client.GetAsync(LocalServices.Kroki).GetAwaiter().GetResult();
                if (!resp.IsSuccessStatusCode)
                {
                    return "Kroki Server not accessible.";
                }

                var content = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!content.Contains("Kroki", StringComparison.CurrentCultureIgnoreCase))
                {
                    return $"There is a server at {LocalServices.Kroki} but seemingly not a Kroki Server.";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return null;
        }
    }
}