using BaTrip.Domain.Entities;
using BaTrip.Domain.Models;
using System.Security.Claims;

namespace BaTrip.Domain.Security
{
    public interface IJwtService    
    {
        TokenPair GenerateTokens(User user);
        ClaimsPrincipal? ValidateAccessToken(string token);
        Guid? GetUserIdFromToken(string token);
    }
}
