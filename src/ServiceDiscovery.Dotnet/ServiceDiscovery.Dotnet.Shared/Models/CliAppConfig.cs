using ServiceDiscovery.Dotnet.Shared.Services.Rabbit;
using ServiceDiscovery.Dotnet.Shared.Services.Redis;

namespace ServiceDiscovery.Dotnet.Shared.Models
{
    public record CliAppConfig
    {
        private RedisConnectionMultiplexer _redisConnectionMultiplexer;
        private RabbitConnection _rabbitConnection;   

        public CliAppConfig(RedisConnectionMultiplexer redisConnectionMultiplexer, RabbitConnection rabbitConnection)
        {
            _redisConnectionMultiplexer = redisConnectionMultiplexer;
            _rabbitConnection = rabbitConnection;
        }

        public string ConfigPathName { get; init; } = string.Empty;
        public string[] Args => [];        

        public async Task<bool> ConnectToRedis(string connectionString)
        {
            var connect = await _redisConnectionMultiplexer.ConnectToRedis(connectionString);
            return connect.IsSuccess;
        }        
    }
}
