using FluentValidation;
using Users.Application.Dtos.Requests;

namespace Users.Application.Validations.Authenticator
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().NotNull()
            .WithMessage("O campo UserName é obrigatório.");

            RuleFor(x => x.Password)
             .NotEmpty().NotNull()
             .WithMessage("O campo Password é obrigatório.");
        }
    }
}
