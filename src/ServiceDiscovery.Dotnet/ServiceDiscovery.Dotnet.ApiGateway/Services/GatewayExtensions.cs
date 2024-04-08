using System.Collections.Immutable;
using Grpc.Core;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;
using DestinationConfig = Yarp.ReverseProxy.Configuration.DestinationConfig;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class GatewayExtensions
{
	public static ClusterConfig ToClusterConfig(this ClusterDto clusterDto) =>
		new()
		{
			ClusterId = clusterDto.ClusterId,
			SessionAffinity = clusterDto.SessionAffinity.ToSessionAffinityConfig(),
			Destinations = clusterDto.Destinations.ToDestinationConfig()
		};

	public static RouteConfig ToRouteConfig(this RouteDto routeDto) =>
		new()
		{
			RouteId = routeDto.RouteId,
			ClusterId = routeDto.ClusterId,
			Match = routeDto?.Match?.ToRouteMatch() ?? new RouteMatch()
		};
	public static RouteMatch ToRouteMatch(this MatchDto matchDto) =>
		new()
		{
			Path = matchDto?.Path ?? string.Empty,
			Hosts = matchDto?.Hosts?.Select( h => h).ToArray() ?? []
		};
	public static SessionAffinityConfig ToSessionAffinityConfig(this SessionAffinityDto sessionAffinityDto) =>
		new()
		{
			Enabled = sessionAffinityDto.Enabled,
			Policy = sessionAffinityDto.Policy,
			AffinityKeyName = sessionAffinityDto.AffinityKeyName
		};
	public static IReadOnlyDictionary<string, DestinationConfig> ToDestinationConfig(this Dictionary<string, DestinationConfigDto> destinationConfigDto)
	{
		Dictionary<string, DestinationConfig> response = [];
		foreach (var entry in destinationConfigDto)
		{			
			response.Add(key: entry.Key,
				value: new DestinationConfig
				{
					Address = entry.Value.Address,
					Metadata = entry.Value.Metadata
				}
			);
		}
		return response;
	}
		
	public static ClusterDto ToClusterDto(this ClusterConfig clusterConfig) =>
		new()
		{
			ClusterId = clusterConfig.ClusterId,
			SessionAffinity = clusterConfig?.SessionAffinity?.ToSessionAffinityDto() ?? new SessionAffinityDto(),
			Destinations = clusterConfig?.Destinations?.ToDestinationConfigDto() ?? []
		};

	public static RouteDto ToRouteDto(this RouteConfig routeConfig) =>
		new()
		{
			RouteId = routeConfig.RouteId,
			ClusterId = routeConfig.ClusterId ?? string.Empty,
			Match = routeConfig.Match.ToMatchDto()
		};

	public static MatchDto ToMatchDto(this RouteMatch routeMatch) =>
		new()
		{
			Path = routeMatch?.Path ?? string.Empty,
			Hosts = routeMatch?.Hosts?.Select(h => h) ?? []
		};

	public static SessionAffinityDto ToSessionAffinityDto(this SessionAffinityConfig sessionAffinityConfig) =>
		new()
		{
			AffinityKeyName = sessionAffinityConfig.AffinityKeyName,
			Enabled	= sessionAffinityConfig?.Enabled ?? true,
			Policy = sessionAffinityConfig?.Policy ?? "Cookie"
		};

	public static Dictionary<string, DestinationConfigDto> ToDestinationConfigDto(this IReadOnlyDictionary<string, DestinationConfig> destinationConfig) =>
     destinationConfig.Any() 
	 	? destinationConfig.Select(dc => 
			(dc.Key,
			new DestinationConfigDto
			{
				Address = dc.Value.Address,
				Metadata = (Dictionary<string, string>)dc.Value.Metadata!

			})).ToDictionary()
		: [];
	public static IReverseProxyBuilder LoadFromRedis(this IReverseProxyBuilder builder, IConfiguration configuration)
	{
#pragma warning disable CA1031 // No capture tipos de excepción generales.
		try 
		{
			
            var connectionString = configuration.GetConnectionString("cache");
            ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString);
			var redis = ConnectionMultiplexer.Connect(connectionString);
            builder.Services.AddSingleton(redis);
			var db = redis.GetDatabase(0);
            var server = redis.GetServer(redis.GetEndPoints().First());
            var routes = server.Keys(database: 0, pattern: "Routes:*");
            var clusters = server.Keys(database: 0, pattern: "Clusters:*");
            var routeConfigs = routes.Select(r => RedisOperations.GetRouteConfigFromValue(redis.GetDatabase().StringGet(r)))
                                    .Where(c => c is not null)
                                    .ToImmutableList() ?? [];
            var clusterConfigs = clusters.Select(c => RedisOperations.GetClusterConfigFromValue(redis.GetDatabase().StringGet(c)))
                                    .Where(c => c is not null)
                                    .ToImmutableList() ?? [];

            builder.Services.AddSingleton(new InMemoryConfigProvider(routeConfigs!, clusterConfigs!));
            builder.Services.AddSingleton((Func<IServiceProvider, IProxyConfigProvider>)((IServiceProvider s) => s.GetRequiredService<InMemoryConfigProvider>()));
        } 
		catch(Exception ex) 
		{
            builder.Services.AddSingleton(new InMemoryConfigProvider([], []));
            builder.Services.AddSingleton((Func<IServiceProvider, IProxyConfigProvider>)((IServiceProvider s) => s.GetRequiredService<InMemoryConfigProvider>()));
        }
#pragma warning restore CA1031 // No capture tipos de excepción generales.

		return builder;
	}
	public static (IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters) GetProxyFromRedis(
		this ConnectionMultiplexer connectionMultiplexer,
		int database = -1,
		string? routePattern = "Routes:*",
		string? clusterPattern = "Clusters:*"
	)
	{
		try
		{
			var server = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().First());
			var routes = server.Keys(database: database, pattern: routePattern);
			var clusters = server.Keys(database: database, pattern: clusterPattern);
			var routeConfigs = routes.Select(r => RedisOperations.GetRouteConfigFromValue(connectionMultiplexer.GetDatabase(database).StringGet(r)))
									.Where(c => c is not null)
									.ToImmutableList() ?? [];
			var clusterConfigs = clusters.Select(c => RedisOperations.GetClusterConfigFromValue(connectionMultiplexer.GetDatabase(database).StringGet(c)))
									.Where(c => c is not null)
									.ToImmutableList() ?? [];
			return (routeConfigs!, clusterConfigs!);
		}
		catch (Exception)
		{
			throw;
		}
	}
}
