using FluentValidation;
using Users.Application.Dtos.Requests;

namespace Users.Application.Validations.User
{
    public class GetUserByIdRequestValidator : AbstractValidator<GetUserByIdRequest>
    {
        public GetUserByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("O campo Id deve ser informado.");
        }
    }
}