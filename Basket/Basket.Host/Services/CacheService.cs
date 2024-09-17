using Basket.Host.Configurations;
using Basket.Host.Services.Interfaces;
using StackExchange.Redis;

namespace Basket.Host.Services
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private readonly IRedisCacheConnectionService _redisCacheConnectionService;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly RedisConfig _config;

        public CacheService(
            ILogger<CacheService> logger,
            IRedisCacheConnectionService redisCacheConnectionService,
            IOptions<RedisConfig> config,
            IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _redisCacheConnectionService = redisCacheConnectionService;
            _jsonSerializer = jsonSerializer;
            _config = config.Value;
        }

        public Task AddOrUpdateAsync<T>(string key, T value) 
        => AddOrUpdateInternalAsync(key, value);

        public async Task<T> GetAsync<T>(string key)
        {
            var redis = GetRedisDatabase();

            var cacheKey = GetItemCacheKey(key);

            var serialized = await redis.StringGetAsync(cacheKey);

            return serialized.HasValue ?
                _jsonSerializer.Deserialize<T>(serialized)
                : default(T)!;
        }

        public async Task ClearData(string key)
        {
            var cacheKey = GetItemCacheKey(key);

            var redis = GetRedisDatabase();

            if (await redis.KeyDeleteAsync(cacheKey))
            {
                _logger.LogInformation($"Cleared data for userId: {key}");
            }
            else
            {
                _logger.LogInformation($"No data found for userId: {key}");
            }
        }

        private string GetItemCacheKey(string userId) =>
            $"{userId}";

        private async Task AddOrUpdateInternalAsync<T>(string key, T value,
    IDatabase redis = null!, TimeSpan? expiry = null)
        {
            redis = redis ?? GetRedisDatabase();
            expiry = expiry ?? _config.CacheTimeout;

            var cacheKey = GetItemCacheKey(key);
            var serialized = _jsonSerializer.Serialize(value);

            await redis.StringSetAsync(cacheKey, serialized, expiry);
        }

        private IDatabase GetRedisDatabase() => _redisCacheConnectionService.Connection.GetDatabase();
    }
}