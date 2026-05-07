using System.Threading;
using System.Threading.Tasks;
using BaTrip.Contracts.Users;

namespace BaTrip.Client.Services;

public interface IAuthGrpcClient
{
    Task<AuthResponse> RegisterAsync(RegistrationRequest request, CancellationToken cancellationToken = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
