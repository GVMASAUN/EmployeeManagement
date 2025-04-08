using System.Text.Json;
using EmployeeManagement.Core.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Core.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheService> _logger;
        public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }
        public async Task DeleteValueAsync(string key, CancellationToken cancellationToken)
        {
            try
            {
                await _cache.RemoveAsync(key, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Cache delete operation canceled.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Cache delete error: {Message}", exception.Message);
            }
        }

        public async Task<T?> GetValueAsync<T>(string key, CancellationToken cancellationToken)
        {
            try
            {
                var cachedData = await _cache.GetStringAsync(key, cancellationToken);

                return string.IsNullOrEmpty(cachedData) ? default : JsonSerializer.Deserialize<T>(cachedData!);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Cache get operation canceled.");
                return default;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Cache get error: {Message}", exception.Message);
                return default;
            }
        }

        public async Task<bool> SetValueAsync<T>(string key, T value, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var options = new DistributedCacheEntryOptions();
                var jsonData = JsonSerializer.Serialize(value, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = false
                });

                options.AbsoluteExpirationRelativeToNow = expirationTime ?? TimeSpan.FromHours(1);

                await _cache.SetStringAsync(key, jsonData, options, cancellationToken);
                return true;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation was canceled while setting cache value for key: {Key}", key);
                return false;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Cache store error: {Message}", exception.Message);
                return false;
            }
        }
    }
}
