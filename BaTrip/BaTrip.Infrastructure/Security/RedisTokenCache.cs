using BaTrip.Domain.Interfaces.Cache;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Runtime;

namespace BaTrip.Infrastructure.Security
{
    public class RedisTokenCache : IRefreshTokenCache
    {
        private readonly IDatabase _redis;
        private readonly JwtSettings _jwtSettings;
        private const string UserTokenKey = "refresh_token:user:";
        private const string TokenUserKey = "refresh_token:token:";

        public RedisTokenCache(IConnectionMultiplexer redis, IOptions<JwtSettings> settings)
        {
            _redis = redis.GetDatabase();
            _jwtSettings = settings.Value;
        }

        public async Task<Guid?> GetUserIdByTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            var userIdStr = await _redis.StringGetAsync($"{TokenUserKey}{refreshToken}");
            return Guid.TryParse(userIdStr.ToString(), out var userId) ? userId : null;
        }

        public async Task<bool> IsValidAsync(Guid userId, string refreshToken, CancellationToken ct = default)
        {
            var storedToken = await _redis.StringGetAsync($"{UserTokenKey}{userId}");
            return !storedToken.IsNullOrEmpty && storedToken == refreshToken;
        }

        public async Task RemoveAsync(Guid userId, CancellationToken ct = default)
        {
            var token = await _redis.StringGetAsync($"{UserTokenKey}{userId}");
            if (!token.IsNullOrEmpty)
            {
                await _redis.KeyDeleteAsync($"{TokenUserKey}{token}");
            }
            await _redis.KeyDeleteAsync($"{UserTokenKey}{userId}");
        }

        public async Task RemoveByTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            var userId = await GetUserIdByTokenAsync(refreshToken, ct);
            if (userId.HasValue)
            {
                await RemoveAsync(userId.Value, ct);
            }
        }

        public async Task SetAsync(Guid userId, string refreshToken, CancellationToken ct = default)
        {
            var ttl = TimeSpan.FromDays(_jwtSettings.RefreshTokenLifetimeDays);

            await _redis.StringSetAsync($"{UserTokenKey}{userId}", refreshToken, ttl);
            await _redis.StringSetAsync($"{TokenUserKey}{refreshToken}", userId.ToString(), ttl);
        }
    }
}
