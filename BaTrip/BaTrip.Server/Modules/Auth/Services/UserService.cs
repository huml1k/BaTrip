using BaTrip.Domain.Entities;
using BaTrip.Domain.Interfaces.Cache;
using BaTrip.Domain.Interfaces.Repositories;
using BaTrip.Domain.Models;
using BaTrip.Domain.Security;
using BaTrip.Server.Modules.Auth.DTOs;
using BaTrip.Server.Modules.Auth.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace BaTrip.Server.Modules.Auth.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenCache _refreshTokenCache;
        private readonly IPasswordHasher _passwordHasher;
        

        public UserService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRefreshTokenCache refreshTokenCache,
            IPasswordHasher passwordHasher) 
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenCache = refreshTokenCache;
            _passwordHasher = passwordHasher;
        }

        public async Task<TokenPair> RegistrationAccountAsync(RegistrationRequestDto request, CancellationToken ct = default)
        {
            var existingUser = await _userRepository.GetByEmail(request.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already registered");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Phone = request.Phone,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = _passwordHasher.Hash(request.Password)
            };

            await _userRepository.Add(user);

            var tokens = _jwtService.GenerateTokens(user);
            await _refreshTokenCache.SetAsync(user.Id, tokens.RefreshToken, ct);

            return tokens;
        }

        public async Task<TokenPair> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmail(request.Email);

            if (user == null || !_passwordHasher.Verify(request.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var tokens = _jwtService.GenerateTokens(user);
            await _refreshTokenCache.SetAsync(user.Id, tokens.RefreshToken, ct);

            return tokens;
        }

        public async Task DeleteUserAsync(
            Guid userId,
            CancellationToken ct = default)
        {
            await _userRepository.Delete(userId);
            await _refreshTokenCache.RemoveAsync(userId, ct);
        }

        public async Task<User> UpdateInformationUserAsync(Guid userId, UpdateProfileRequestDto request, CancellationToken ct = default)
        {
            var user = await _userRepository.GetById(userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;

            await _userRepository.Update(user);

            return user;
        }

        public async Task<User> GetUserProfileAsync(
            Guid userId,
            CancellationToken ct = default)
        {
            var user = await _userRepository.GetById(userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return user;
        }
    }
}
