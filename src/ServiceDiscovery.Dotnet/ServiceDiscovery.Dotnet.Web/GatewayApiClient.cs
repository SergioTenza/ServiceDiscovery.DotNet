using Fluxor;
using ServiceDiscovery.Dotnet.Shared;

namespace ServiceDiscovery.Dotnet.Web;

public class GatewayApiClient(HttpClient httpClient,IHttpClientFactory httpClientFactory)
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    public async Task<RouteDto[]> GetRoutesAsync()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<RouteDto[]>("/routes") ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return [];
        }

    }
    public async Task<ClusterDto[]> GetClustersAsync()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<ClusterDto[]>("/clusters") ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return [];
        }
    }
    [EffectMethod]
    public async Task HandleFetchRoutesAction(FetchRoutesAction action, IDispatcher dispatcher)
    {

        try
        {             
            var httpClient = _httpClientFactory.CreateClient("Gateway");            
            var routes = await httpClient.GetFromJsonAsync<RouteDto[]>("/routes") ?? [];
            dispatcher.Dispatch(new FetchRoutesResultAction(routes));
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
        }
    }
}