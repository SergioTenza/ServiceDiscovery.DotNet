using Rop.Result.Object;
using StackExchange.Redis;

namespace ServiceDiscovery.Dotnet.Shared.Services.Redis
{    
    public class RedisConnectionMultiplexer
    {
        private ConnectionMultiplexer? _multiplexer;


        public async Task<Result<bool>> ConnectToRedis(string connectionString)            
        {
            try
            {
                if (_multiplexer is not null)
                {
                    return true;
                }
                _multiplexer = await ConnectionMultiplexer.ConnectAsync(connectionString);
                return true;
            }
            catch (Exception ex)
            {
                return Result<bool>.Failed(new Error 
                {
                    DomainError = DomainError.RedisServerError,
                    Code = DomainError.RedisServerError.ToString(),
                    Message = ex.Message
                });
            }
        }
    }
}
