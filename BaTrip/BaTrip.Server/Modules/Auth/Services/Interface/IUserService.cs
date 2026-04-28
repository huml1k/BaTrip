using BaTrip.Domain.Entities;
using BaTrip.Domain.Models;
using BaTrip.Server.Modules.Auth.DTOs;

namespace BaTrip.Server.Modules.Auth.Services.Interface
{
    public interface IUserService
    {
        Task<TokenPair> RegistrationAccountAsync(RegistrationRequestDto request,CancellationToken ct = default);

        Task<TokenPair> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
      
        Task DeleteUserAsync(Guid userId, CancellationToken ct = default);

        Task<User> UpdateInformationUserAsync(Guid userId, UpdateProfileRequestDto request, CancellationToken ct = default);

        Task<User> GetUserProfileAsync(Guid userId, CancellationToken ct = default);
    }
}
