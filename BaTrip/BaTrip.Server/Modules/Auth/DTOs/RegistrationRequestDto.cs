namespace BaTrip.Server.Modules.Auth.DTOs
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public int Phone { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
