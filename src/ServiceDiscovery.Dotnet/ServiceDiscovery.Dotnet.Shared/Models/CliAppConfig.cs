using ServiceDiscovery.Dotnet.Shared.Services.Rabbit;
using ServiceDiscovery.Dotnet.Shared.Services.Redis;

namespace ServiceDiscovery.Dotnet.Shared.Models
{
    public record CliAppConfig
    {
        private readonly RedisConnectionMultiplexer _redisConnectionMultiplexer;        

        public CliAppConfig(RedisConnectionMultiplexer redisConnectionMultiplexer)
        {
            _redisConnectionMultiplexer = redisConnectionMultiplexer;
        }

        public string ConfigPathName { get; init; } = string.Empty;
        public string[] Args => [];        

        public async Task<bool> ConnectToRedis(string connectionString)
        {
            var connect = await _redisConnectionMultiplexer.ConnectToRedis(connectionString).ConfigureAwait(true);
            return connect.IsSuccess;
        }        
    }
}
