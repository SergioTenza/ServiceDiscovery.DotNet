using ServiceDiscovery.Dotnet.Shared.Services.Rabbit;
using ServiceDiscovery.Dotnet.Shared.Services.Redis;

namespace ServiceDiscovery.Dotnet.Shared.Models
{
    public record CliAppConfig
    {
        private RedisConnectionMultiplexer _redisConnectionMultiplexer;
        private RabbitConnection _rabbitConnection;
        private readonly CliConfig _cliConfig;        

        public CliAppConfig(RedisConnectionMultiplexer redisConnectionMultiplexer, RabbitConnection rabbitConnection, CliConfig cliConfig)
        {
            _redisConnectionMultiplexer = redisConnectionMultiplexer;
            _rabbitConnection = rabbitConnection;
            _cliConfig = cliConfig;
        }

        public string ConfigPathName { get; init; } = string.Empty;
        public string[] Args => _cliConfig.Args ?? [];        

        public async Task<bool> ConnectToRedis(string connectionString)
        {
            var connect = await _redisConnectionMultiplexer.ConnectToRedis(connectionString);
            return connect.IsSuccess;
        }        
    }
}
