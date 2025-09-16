using FluentValidation;
using Users.Application.Dtos.Requests;

namespace Users.Application.Validations.User
{
    public class GetUserByNickNameRequestValidator : AbstractValidator<GetUserByNickNameRequest>
    {
        public GetUserByNickNameRequestValidator()
        {
            RuleFor(x => x.NickName)
                .NotNull()
                .NotEmpty()
                .WithMessage("O campo NickName deve ser informado.");
        }
    }
}
