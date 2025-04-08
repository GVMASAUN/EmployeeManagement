namespace EmployeeManagement.Core.Contracts
{
    public interface IRedisCacheService
    {
        public Task DeleteValueAsync(string key, CancellationToken cancellationToken);
        public Task<T?> GetValueAsync<T>(string key, CancellationToken cancellationToken);
        public Task<bool> SetValueAsync<T>(string key, T value, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default);
    }
}
