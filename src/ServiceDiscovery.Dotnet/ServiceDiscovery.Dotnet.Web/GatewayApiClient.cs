using Fluxor;
using ServiceDiscovery.Dotnet.Shared;

namespace ServiceDiscovery.Dotnet.Web;

public class GatewayApiClient(HttpClient httpClient, IHttpClientFactory httpClientFactory)
{
	private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

	public async Task<RouteDto[]> GetRoutesAsync()
	{
		return await httpClient.GetFromJsonAsync<RouteDto[]>("/routes").ConfigureAwait(false) ?? [];
	}

	public async Task<ClusterDto[]> GetClustersAsync()
	{
		return await httpClient.GetFromJsonAsync<ClusterDto[]>("/clusters").ConfigureAwait(false) ?? [];
	}

	[EffectMethod]
	public async Task HandleFetchRoutesAction(FetchRoutesAction action, IDispatcher dispatcher)
	{
		var httpClient = _httpClientFactory.CreateClient("Gateway");
		if (httpClient is not null)
		{
			var routes = await httpClient.GetFromJsonAsync<RouteDto[]>("/routes").ConfigureAwait(false) ?? [];
			ArgumentNullException.ThrowIfNull(dispatcher);
			dispatcher.Dispatch(new FetchRoutesResultAction(routes));
		}
	}
}
