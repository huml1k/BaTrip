using BaTrip.Contracts.Users;
using BaTrip.Domain.Security;
using BaTrip.Server.Modules.Auth.Mapper.Interfaces;
using BaTrip.Server.Modules.Auth.Services.Interface;
using BaTrip.Server.Modules.Auth.Validators;
using FluentValidation;
using Grpc.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Grpc.AspNetCore.Server;
using Mapster;
using BaTrip.Server.Modules.Auth.DTOs;

namespace BaTrip.Server.Modules.Auth
{
    public class AuthGrpcService : UserService.UserServiceBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IAuthMapper _authMapper;
        private readonly IValidator<RegistrationRequest> _registrationValidator;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<UpdateProfileRequest> _updateProfileValidator;

        public AuthGrpcService(
            IUserService userService,
            IJwtService jwtService,
            IAuthMapper authMapper,
            IValidator<RegistrationRequest> registrationValidator,
            IValidator<LoginRequest> loginValidator,
            IValidator<UpdateProfileRequest> updateProfileValidator
            ) 
        {
            _userService = userService;
            _jwtService = jwtService;
            _authMapper = authMapper;
            _registrationValidator = registrationValidator;
            _loginValidator = loginValidator;
            _updateProfileValidator = updateProfileValidator;
        }

        public override async Task<AuthResponse> RegistrationProfile(RegistrationRequest request, ServerCallContext context)
        {
            var validationResult = await _registrationValidator.ValidateAsync(request, context.CancellationToken);
            if (!validationResult.IsValid)
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))));
            var dto = request.Adapt<RegistrationRequestDto>();
            var tokens = await _userService.RegistrationAccountAsync(dto, context.CancellationToken);
            var userId = _jwtService.GetUserIdFromToken(tokens.AccessToken) ?? throw new RpcException(new Status(StatusCode.Internal, "Token generation failed"));
            var user = await _userService.GetUserProfileAsync(userId, context.CancellationToken);

            return _authMapper.ToAuthResponse(tokens, user);
        }

        public override async Task<AuthResponse> LoginProfile(LoginRequest request, ServerCallContext context)
        {
            var validationResult = await _loginValidator.ValidateAsync(request, context.CancellationToken);
            if (!validationResult.IsValid)
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))));

            var dto = request.Adapt<LoginRequestDto>();
            var tokens = await _userService.LoginAsync(dto, context.CancellationToken);
            var userId = _jwtService.GetUserIdFromToken(tokens.AccessToken) ?? throw new RpcException(new Status(StatusCode.Internal, "Token generation failed"));
            var user = await _userService.GetUserProfileAsync(userId, context.CancellationToken);

            return _authMapper.ToAuthResponse(tokens, user);
        }

        public override async Task<UserProfileResponse> UpdateProfile(UpdateProfileRequest request, ServerCallContext context)
        {
            var validationResult = await _updateProfileValidator.ValidateAsync(request, context.CancellationToken);
            if (!validationResult.IsValid)
                throw new RpcException(new Status(StatusCode.InvalidArgument, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))));

            var dto = request.Adapt<UpdateProfileRequestDto>();
            var userId = GetUserIdFromContext(context);
            var user = await _userService.UpdateInformationUserAsync(userId, dto, context.CancellationToken);
            return _authMapper.ToUserProfileResponse(user);
        }

        public override async Task<DeleteProfileResponse> DeleteProfile(DeleteProfileRequest request, ServerCallContext context)
        {
            var userId = GetUserIdFromContext(context);
            await _userService.DeleteUserAsync(userId, context.CancellationToken);
            return new DeleteProfileResponse { Success = true, Message = "Profile deleted successfully" };
        }

        public override async Task<UserProfileResponse> GetProfile(GetProfileRequest request, ServerCallContext context)
        {
            var userId = GetUserIdFromContext(context);
            var user = await _userService.GetUserProfileAsync(userId, context.CancellationToken);
            return _authMapper.ToUserProfileResponse(user);
        }

        private Guid GetUserIdFromContext(ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            var claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(claim, out var userId))
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid or missing authentication token"));
            return userId;
        }
    }
}
