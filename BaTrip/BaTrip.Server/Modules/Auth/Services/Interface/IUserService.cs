using BaTrip.Domain.Entities;
using BaTrip.Domain.Models;

namespace BaTrip.Server.Modules.Auth.Services.Interface
{
    public interface IUserService
    {
        Task<TokenPair> RegistrationAccountAsync(
            string email,
            int phone,
            string firstName,
            string lastName,
            string password,
            CancellationToken ct = default);

        Task<TokenPair> LoginAsync(
            string email,
            string password,
            CancellationToken ct = default);
      
        Task DeleteUserAsync(Guid userId, CancellationToken ct = default);

        Task<User> UpdateInformationUserAsync(
            Guid userId,
            string firstName,
            string lastName,
            CancellationToken ct = default);

        Task<User> GetUserProfileAsync(Guid userId, CancellationToken ct = default);
    }
}
