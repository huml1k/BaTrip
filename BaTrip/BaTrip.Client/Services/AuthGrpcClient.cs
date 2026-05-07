using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BaTrip.Contracts.Users;
using Grpc.Net.Client;

namespace BaTrip.Client.Services;

public sealed class AuthGrpcClient : IAuthGrpcClient
{
    private readonly UserService.UserServiceClient _client;

    public AuthGrpcClient(string serverAddress = "https://localhost:7170")
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var channel = GrpcChannel.ForAddress(serverAddress, new GrpcChannelOptions
        {
            HttpHandler = handler
        });

        _client = new UserService.UserServiceClient(channel);
    }

    public async Task<AuthResponse> RegisterAsync(RegistrationRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.RegistrationProfileAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        return await _client.LoginProfileAsync(request, cancellationToken: cancellationToken).ResponseAsync;
    }
}
