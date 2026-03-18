using FluentValidation;
using shipman.Server.Application.Dtos.Shipments;

namespace shipman.Server.Application.Validators;

public class ContactDtoValidator : AbstractValidator<ContactDto>
{
    public ContactDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Phone)
            .NotEmpty();

        RuleFor(x => x.Address)
            .NotNull()
            .SetValidator(new AddressDtoValidator());
    }
}
