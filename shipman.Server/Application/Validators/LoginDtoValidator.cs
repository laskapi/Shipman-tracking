using FluentValidation;
using shipman.Server.Application.Dtos.Auth;

namespace shipman.Server.Application.Validators;
public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

