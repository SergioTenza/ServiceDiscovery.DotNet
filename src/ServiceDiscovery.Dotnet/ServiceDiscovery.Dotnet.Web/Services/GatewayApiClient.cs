using Fluxor;
using Microsoft.FluentUI.AspNetCore.Components;
using ServiceDiscovery.Dotnet.Shared;

namespace ServiceDiscovery.Dotnet.Web;

public class GatewayApiClient(IHttpClientFactory httpClientFactory, IMessageService messageService)
{
    private readonly IMessageService _messageService = messageService;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    // [EffectMethod]
    // public async Task HandleFetchRoutesAction(FetchRoutesAction action, IDispatcher dispatcher)
    // {
    // 	var httpClient = httpClientFactory.CreateClient("Gateway");
    // 	if (httpClient is not null)
    // 	{
    // 		try
    // 		{
    // 			var routes = await httpClient.GetFromJsonAsync<RouteDto[]>("/routes") ?? [];	
    // 			ArgumentNullException.ThrowIfNull(dispatcher);
    // 			dispatcher.Dispatch(new FetchRoutesResultAction(routes));
    // 			await Task.CompletedTask;
    // 		}
    // 		catch(Exception ex)
    // 		{				
    // 			Console.WriteLine(ex.Message);
    // 			ArgumentNullException.ThrowIfNull(dispatcher);
    // 			dispatcher.Dispatch(new FetchRoutesResultAction([]));
    // 			await Task.CompletedTask;
    // 		}
    // 	}
    // }
    // [EffectMethod]
    // public async Task HandleFetchClustersAction(FetchClustersAction action, IDispatcher dispatcher)
    // {
    // 	var httpClient = httpClientFactory.CreateClient("Gateway");
    // 	if (httpClient is not null)
    // 	{
    // 		try
    // 		{
    // 			var clusters = await httpClient.GetFromJsonAsync<ClusterDto[]>("/clusters") ?? [];	
    // 			ArgumentNullException.ThrowIfNull(dispatcher);
    // 			dispatcher.Dispatch(new FetchClustersResultAction(clusters));
    // 		}
    // 		catch(Exception ex)
    // 		{
    // 				Console.WriteLine(ex.Message);
    // 			ArgumentNullException.ThrowIfNull(dispatcher);
    // 			dispatcher.Dispatch(new FetchClustersResultAction([]));
    // 			await Task.CompletedTask;

    // 		}
    // 	}
    // }

    [EffectMethod]
    public async Task HandleFetchGatewayAction(FetchGatewayAction action, IDispatcher dispatcher)
    {
        var httpClient = _httpClientFactory.CreateClient("Gateway");
        if (httpClient is not null)
        {
            try
            {
                var clusters = await httpClient.GetFromJsonAsync<GatewayConfig>("/v1/gateway");
                ArgumentNullException.ThrowIfNull(dispatcher);
                if (clusters is not null)
                {
                    dispatcher.Dispatch(new FetchGatewayResultAction(clusters.Routes, clusters.Clusters));
                    return;
                }
                dispatcher.Dispatch(new FetchGatewayResultAction([], []));
                await Task.CompletedTask;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ArgumentNullException.ThrowIfNull(dispatcher);
                dispatcher.Dispatch(new FetchClustersResultAction([]));
                await Task.CompletedTask;

            }
        }
    }

    [EffectMethod]
    public async Task HandlePostNewRouteAction(PostNewRouteAction action, IDispatcher dispatcher)
    {
        var httpClient = _httpClientFactory.CreateClient("Gateway");
        if (httpClient is not null)
        {
            try
            {
                var posted = await httpClient.PostAsJsonAsync("/v1/routes", action.Dto);
                ArgumentNullException.ThrowIfNull(dispatcher);
                if (posted is not null && posted.IsSuccessStatusCode)
                {
                    dispatcher.Dispatch(new PostNewRouteResultAction
                    {
                        Dto = action.Dto
                    });
					var message = $"Route {action.Dto.RouteId} successfully added to gateway";
        			var type = MessageIntent.Success;
        			await _messageService.ShowMessageBarAsync(message, type, "TOP");
					dispatcher.Dispatch(new FetchGatewayAction());

                }
                else
                {
                    var message = $"Cannot add route to gateway<br/> {posted?.StatusCode}\n{posted?.ReasonPhrase}";
        			var type = MessageIntent.Error;
        			await _messageService.ShowMessageBarAsync(message, type, "TOP");
                }               

            }
            catch (Exception ex)
            {
                var message = $"Cannot add route to gateway\n 500\n{ex.Message}";
				var type = MessageIntent.Error;
				await _messageService.ShowMessageBarAsync(message, type, "TOP");
            }
			await Task.CompletedTask;
        }
    }
}
