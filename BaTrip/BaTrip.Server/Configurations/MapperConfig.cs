using BaTrip.Contracts.Users;
using BaTrip.Domain.Entities;
using BaTrip.Server.Modules.Auth.DTOs;
using Mapster;

namespace BaTrip.Server.Configurations
{
    public static class MapperConfig
    {
        public static void Register() 
        {
            TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(false);

            TypeAdapterConfig<RegistrationRequest, RegistrationRequestDto>.NewConfig()
                .Map(d => d.Email, s => s.Email)
                .Map(d => d.Phone, s => s.Phone)
                .Map(d => d.FirstName, s => s.Firstname)
                .Map(d => d.LastName, s => s.Lastname)
                .Map(d => d.Password, s => s.Password)
                .IgnoreNonMapped(true);

            TypeAdapterConfig<LoginRequest, LoginRequestDto>.NewConfig()
                .Map(d => d.Email, s => s.Email)
                .Map(d => d.Password, s => s.Password);

            TypeAdapterConfig<UpdateProfileRequest, UpdateProfileRequestDto>.NewConfig()
                .Map(d => d.FirstName, s => s.Firstname)
                .Map(d => d.LastName, s => s.Lastname)
                .IgnoreNonMapped(true);

            TypeAdapterConfig<User, UserProfileResponse>.NewConfig()
                .Map(d => d.Firstname, s => s.FirstName)
                .Map(d => d.Lastname, s => s.LastName);
        }
    }
}
