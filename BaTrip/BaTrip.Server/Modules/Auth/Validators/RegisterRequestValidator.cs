using FluentValidation;
using BaTrip.Contracts.Users;

namespace BaTrip.Server.Modules.Auth.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegistrationRequest>
    {
        public RegisterRequestValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязательный")
                .EmailAddress().WithMessage("Неверный формат email");
            RuleFor(x => x.Phone)
                .GreaterThan(0).WithMessage("Номер телефона должен быть указан");
            RuleFor(x => x.Firstname)
                .NotEmpty().WithMessage("Имя обязательно")
                .MaximumLength(32).WithMessage("Максимальная длина имени: 32 символов");
            RuleFor(x => x.Lastname)
                .NotEmpty().WithMessage("Фамилия обязательна")
                .MaximumLength(20).WithMessage("Максимальная длина фамилии: 20 символов");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(8).WithMessage("Минимальная длина пароля: 8 символов")
                .MaximumLength(16).WithMessage("Максимальная длина пароля: 16 символов");
        }
    }
}
