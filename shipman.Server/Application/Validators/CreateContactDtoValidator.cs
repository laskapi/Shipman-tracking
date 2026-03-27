using FluentValidation;
using shipman.Server.Application.Dtos.Contacts;

namespace shipman.Server.Application.Validators;

public class CreateContactDtoValidator : AbstractValidator<CreateContactDto>
{
    public CreateContactDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Phone).NotEmpty();

        RuleFor(x => x.PrimaryAddress)
            .NotNull()
            .SetValidator(new CreateAddressDtoValidator());
    }
}
