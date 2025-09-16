using FluentValidation;
using Users.Application.Dtos.Requests;

namespace Users.Application.Validations.User
{
    public class GetUserByEmailRequestValidator : AbstractValidator<GetUserByEmailRequest>
    {
        public GetUserByEmailRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("O campo Email deve conter um endereço de e-mail válido.");
        }
    }
}
