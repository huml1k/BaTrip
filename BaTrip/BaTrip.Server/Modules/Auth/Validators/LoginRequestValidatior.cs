using BaTrip.Contracts.Users;
using FluentValidation;

namespace BaTrip.Server.Modules.Auth.Validators
{
    public class LoginRequestValidatior : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidatior()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Неверный формат Email");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен");
        }
    }
}
