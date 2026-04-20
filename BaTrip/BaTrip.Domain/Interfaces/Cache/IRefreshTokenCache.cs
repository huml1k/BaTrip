namespace BaTrip.Domain.Interfaces.Cache
{
    public interface IRefreshTokenCache
    {
        Task SetAsync(Guid userId, string refreshToken, CancellationToken ct = default);
        Task<bool> IsValidAsync(Guid userId, string refreshToken, CancellationToken ct = default);
        Task<Guid?> GetUserIdByTokenAsync(string refreshToken, CancellationToken ct = default);
        Task RemoveAsync(Guid userId, CancellationToken ct = default);
        Task RemoveByTokenAsync(string refreshToken, CancellationToken ct = default);
    }
}
