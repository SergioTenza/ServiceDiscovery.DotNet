using ServiceDiscovery.Dotnet.Shared;

namespace ServiceDiscovery.Dotnet.Web;

public class GatewayApiClient(HttpClient httpClient)
{
    public async Task<RouteDto[]> GetRoutesAsync()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<RouteDto[]>("/routes") ?? [];
        }
        catch(Exception ex)
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
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return [];
        }
    }
}