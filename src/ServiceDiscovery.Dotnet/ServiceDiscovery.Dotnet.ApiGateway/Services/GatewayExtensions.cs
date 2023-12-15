﻿using System.Collections.Immutable;
using ServiceDiscovery.Dotnet.Shared;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class GatewayExtensions
{
	public static ClusterConfig ToClusterConfig(this ClusterDto clusterDto) =>
		new()
		{
			ClusterId = clusterDto.ClusterId,
			SessionAffinity = clusterDto.SessionAffinity.ToSessionAffinityConfig(),
			Destinations = clusterDto.Destination.ToSessionAffinityConfig()
		};

	public static RouteConfig ToRouteConfig(this RouteDto routeDto) =>
		new()
		{
			RouteId = routeDto.RouteId,
			ClusterId = routeDto.ClusterId,
			Match = routeDto.Match.ToRouteMatch()
		};
	public static RouteMatch ToRouteMatch(this MatchDto matchDto) =>
		new()
		{

		};
	public static SessionAffinityConfig ToSessionAffinityConfig(this SessionAffinityDto sessionAffinityDto) =>
		new()
		{

		};
	public static IReadOnlyDictionary<string, DestinationConfig> ToSessionAffinityConfig(this Dictionary<string, DestinationConfigDto> destinationConfigDto) =>
		new Dictionary<string, DestinationConfig>
		{

		};
	public static ClusterDto ToClusterDto(this ClusterConfig clusterConfig) =>
		new()
		{
			ClusterId = clusterConfig.ClusterId,
			SessionAffinity = clusterConfig?.SessionAffinity?.ToSessionAffinityDto() ?? new SessionAffinityDto(),
			Destination = clusterConfig?.Destinations?.ToDestinationConfigDto() ?? []
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

		};

	public static SessionAffinityDto ToSessionAffinityDto(this SessionAffinityConfig sessionAffinityConfig) =>
		new()
		{

		};

	public static Dictionary<string, DestinationConfigDto> ToDestinationConfigDto(this IReadOnlyDictionary<string, DestinationConfig> destinationConfig) =>
	new()
	{

	};
	public static IReverseProxyBuilder LoadFromRedis(this IReverseProxyBuilder builder, IConfiguration configuration)
	{
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
		catch 
		{
            builder.Services.AddSingleton(new InMemoryConfigProvider([], []));
            builder.Services.AddSingleton((Func<IServiceProvider, IProxyConfigProvider>)((IServiceProvider s) => s.GetRequiredService<InMemoryConfigProvider>()));
        }
		
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