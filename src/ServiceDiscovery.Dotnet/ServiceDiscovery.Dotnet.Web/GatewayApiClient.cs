using Fluxor;
using ServiceDiscovery.Dotnet.Shared;

namespace ServiceDiscovery.Dotnet.Web;

public class GatewayApiClient(IHttpClientFactory httpClientFactory)
{
    private readonly IHttpClientFactory httpClientFactory = httpClientFactory;

    [EffectMethod]
	public async Task HandleFetchRoutesAction(FetchRoutesAction action, IDispatcher dispatcher)
	{
		var httpClient = httpClientFactory.CreateClient("Gateway");
		if (httpClient is not null)
		{
			try
			{
				var routes = await httpClient.GetFromJsonAsync<RouteDto[]>("/routes") ?? [];	
				ArgumentNullException.ThrowIfNull(dispatcher);
				dispatcher.Dispatch(new FetchRoutesResultAction(routes));
				await Task.CompletedTask;
			}
			catch(Exception ex)
			{				
				Console.WriteLine(ex.Message);
				ArgumentNullException.ThrowIfNull(dispatcher);
				dispatcher.Dispatch(new FetchRoutesResultAction([]));
				await Task.CompletedTask;
			}
		}
	}
	[EffectMethod]
	public async Task HandleFetchClustersAction(FetchClustersAction action, IDispatcher dispatcher)
	{
		var httpClient = httpClientFactory.CreateClient("Gateway");
		if (httpClient is not null)
		{
			try
			{
				var clusters = await httpClient.GetFromJsonAsync<ClusterDto[]>("/clusters") ?? [];	
				ArgumentNullException.ThrowIfNull(dispatcher);
				dispatcher.Dispatch(new FetchClustersResultAction(clusters));
			}
			catch(Exception ex)
			{
					Console.WriteLine(ex.Message);
				ArgumentNullException.ThrowIfNull(dispatcher);
				dispatcher.Dispatch(new FetchClustersResultAction([]));
				await Task.CompletedTask;

			}
		}
	}
}
