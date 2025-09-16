using FluentValidation;
using Users.Application.Dtos.Requests;

namespace Users.Application.Validations.User
{
    public class DeleteUserRequestValidator : AbstractValidator<DeleteUserRequest>
    {
        public DeleteUserRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .WithMessage("O campo Id deve ser informado.");
        }
    }
}