using BaTrip.Contracts.Users;
using BaTrip.Domain.Entities;
using BaTrip.Domain.Models;

namespace BaTrip.Server.Modules.Auth.Mapper.Interfaces
{
    public interface IAuthMapper
    {
        UserProfileResponse ToUserProfileResponse(User user);
        AuthResponse ToAuthResponse(TokenPair tokens, User user);
    }
}
