using BaTrip.Domain.Entities;
using BaTrip.Domain.Interfaces.Cache;
using BaTrip.Domain.Interfaces.Repositories;
using BaTrip.Domain.Models;
using BaTrip.Domain.Security;
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

        public async Task<TokenPair> RegistrationAccountAsync(
            string email,
            int phone,
            string firstName,
            string lastName,
            string password,
            CancellationToken ct = default)
        {
            var existingUser = await _userRepository.GetByEmail(email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already registered");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                Phone = phone,
                FirstName = firstName,
                LastName = lastName,
                Password = _passwordHasher.Hash(password)
            };

            await _userRepository.Add(user);

            var tokens = _jwtService.GenerateTokens(user);
            await _refreshTokenCache.SetAsync(user.Id, tokens.RefreshToken, ct);

            return tokens;
        }

        public async Task<TokenPair> LoginAsync(
            string email,
            string password,
            CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmail(email);

            if (user == null || !_passwordHasher.Verify(password, user.Password))
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

        public async Task<User> UpdateInformationUserAsync(
            Guid userId,
            string firstName,
            string lastName,
            CancellationToken ct = default)
        {
            var user = await _userRepository.GetById(userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            user.FirstName = firstName;
            user.LastName = lastName;

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
