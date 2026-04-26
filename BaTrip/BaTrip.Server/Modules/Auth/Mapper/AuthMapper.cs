using BaTrip.Contracts.Users;
using BaTrip.Domain.Entities;
using BaTrip.Domain.Models;
using BaTrip.Server.Modules.Auth.Mapper.Interfaces;
using Mapster;

namespace BaTrip.Server.Modules.Auth.Mapper
{
    public class AuthMapper : IAuthMapper
    {
        public AuthResponse ToAuthResponse(TokenPair tokens, User user)
            => new AuthResponse
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                ExpiresIn = tokens.ExpiresIn,
                User = ToUserProfileResponse(user)
            };

        public UserProfileResponse ToUserProfileResponse(User user) => user.Adapt<UserProfileResponse>();
    }
}
