using BaTrip.Domain.Interfaces.Cache;
using BaTrip.Domain.Interfaces.Repositories;
using BaTrip.Domain.Security;
using BaTrip.Infrastructure.Data;
using BaTrip.Infrastructure.Data.Repositories;
using BaTrip.Infrastructure.Security;
using BaTrip.Server.Configurations;
using BaTrip.Server.Modules.Auth;
using BaTrip.Server.Modules.Auth.Mapper;
using BaTrip.Server.Modules.Auth.Mapper.Interfaces;
using BaTrip.Server.Modules.Auth.Services;
using BaTrip.Server.Modules.Auth.Services.Interface;
using BaTrip.Server.Modules.Auth.Validators;
using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

MapperConfig.Register();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
    );

var redisConnection = builder.Configuration.GetConnectionString("Redis")
                      ?? "localhost:6379";

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(redisConnection)
);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidAudience = jwtSettings?.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key ?? throw new InvalidOperationException("Jwt:Key is missing")))
    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IRefreshTokenCache, RedisTokenCache>();


//gRPC
builder.Services.AddGrpc();

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>(ServiceLifetime.Scoped);
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidatior>(ServiceLifetime.Scoped);
builder.Services.AddValidatorsFromAssemblyContaining<UpdateProfileRequestValidator>(ServiceLifetime.Scoped);

//Mapper
builder.Services.AddSingleton<IAuthMapper, AuthMapper>();


// Repositories
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();

// Services
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<AuthGrpcService>();

app.Run();
