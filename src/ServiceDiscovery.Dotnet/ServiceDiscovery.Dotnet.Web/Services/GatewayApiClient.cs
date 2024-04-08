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
#pragma warning disable IDE0060 // Quitar el parámetro no utilizado
	public async Task HandleFetchGatewayAction(FetchGatewayAction action, IDispatcher dispatcher)
#pragma warning restore IDE0060 // Quitar el parámetro no utilizado
	{
		var httpClient = _httpClientFactory.CreateClient("Gateway");
		if (httpClient is not null)
		{

#pragma warning disable CA1031 // No capture tipos de excepción generales.
			try
			{
				var clusters = await httpClient.GetFromJsonAsync<ReverseProxy>("/v1/gateway").ConfigureAwait(true);
				ArgumentNullException.ThrowIfNull(dispatcher);
				if (clusters is not null)
				{
					dispatcher.Dispatch(new FetchGatewayResultAction(clusters.Routes, clusters.Clusters));
					return;
				}
				dispatcher.Dispatch(new FetchGatewayResultAction([], []));
				await Task.CompletedTask.ConfigureAwait(true);

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				ArgumentNullException.ThrowIfNull(dispatcher);
				dispatcher.Dispatch(new FetchClustersResultAction([]));
				await Task.CompletedTask.ConfigureAwait(true);

			}
#pragma warning restore CA1031 // No capture tipos de excepción generales.

		}
	}

	[EffectMethod]
	public async Task HandlePostNewRouteAction(PostNewRouteAction action, IDispatcher dispatcher)
	{
		var httpClient = this._httpClientFactory.CreateClient("Gateway");
		if (httpClient is not null)
		{
#pragma warning disable CA1031 // No capture tipos de excepción generales.
			try
			{
				var posted = await httpClient.PostAsJsonAsync("/v1/routes", action.Dto).ConfigureAwait(true);
				ArgumentNullException.ThrowIfNull(dispatcher);
				if (posted is not null && posted.IsSuccessStatusCode)
				{
					dispatcher.Dispatch(new PostNewRouteResultAction
					{
						Dto = action.Dto
					});
					var message = $"Route {action.Dto.RouteId} successfully added to gateway";
					var type = MessageIntent.Success;
					_ = await this._messageService.ShowMessageBarAsync(message, type, "TOP").ConfigureAwait(true);
					dispatcher.Dispatch(new FetchGatewayAction());

				}
				else
				{
					var message = $"Cannot add route to gateway<br/> {posted?.StatusCode}\n{posted?.ReasonPhrase}";
					var type = MessageIntent.Error;
					_ = await this._messageService.ShowMessageBarAsync(message, type, "TOP").ConfigureAwait(true);
				}

			}
			catch (Exception ex)
			{
				var message = $"Cannot add route to gateway\n 500\n{ex.Message}";
				var type = MessageIntent.Error;
				_ = await _messageService.ShowMessageBarAsync(message, type, "TOP").ConfigureAwait(true);
			}
#pragma warning restore CA1031 // No capture tipos de excepción generales.
			await Task.CompletedTask.ConfigureAwait(true);
		}
	}

	public async Task<ReverseProxy> GetGatewayConfigAsJson(Exception exception)
	{
		var httpClient = this._httpClientFactory.CreateClient("Gateway");
		if (httpClient is not null)
		{
			try
			{
				var petition = await httpClient.GetFromJsonAsync<ReverseProxy>("/v1/gateway").ConfigureAwait(true);
				return petition ?? throw exception;

			}
			catch (Exception ex)
			{
				var message = $"Cannot get config from gateway configuration\n 500\n{ex.Message}";
				var type = MessageIntent.Error;
				_ = await this._messageService.ShowMessageBarAsync(message, type, "TOP").ConfigureAwait(true);
				throw;
			}
		}
		throw new InvalidOperationException("Cannot get config from gateway configuration");
	}
}
