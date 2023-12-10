using System.Text.Json;
using StackExchange.Redis;
using Yarp.ReverseProxy.Configuration;

namespace ServiceDiscovery.Dotnet.ApiGateway;

public static class RedisOperations
{
    public static readonly Func<RedisValue, RouteConfig?> GetRouteConfigFromValue = (value) =>
        {
            if (value.HasValue && !value.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<RouteConfig>(value.ToString());
            }
            return null;
        };
    public static readonly Func<RedisValue, ClusterConfig?> GetClusterConfigFromValue = (value) =>
    {
        if (value.HasValue && !value.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<ClusterConfig>(value.ToString());
        }
        return null;
    };
}
