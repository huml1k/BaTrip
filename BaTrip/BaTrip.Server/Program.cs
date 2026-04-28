using BaTrip.Domain.Interfaces.Repositories;
using BaTrip.Domain.Security;
using BaTrip.Infrastructure.Data;
using BaTrip.Infrastructure.Data.Repositories;
using BaTrip.Infrastructure.Security;
using BaTrip.Server.Modules.Auth;
using BaTrip.Server.Modules.Auth.Mapper;
using BaTrip.Server.Modules.Auth.Mapper.Interfaces;
using BaTrip.Server.Modules.Auth.Services;
using BaTrip.Server.Modules.Auth.Services.Interface;
using BaTrip.Server.Modules.Auth.Validators;
using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.EntityFrameworkCore;
using BaTrip.Server.Configurations;

var builder = WebApplication.CreateBuilder(args);

MapperConfig.Register();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
    );

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
