using FluentValidation;
using shipman.Server.Application.Dtos.Contacts;

namespace shipman.Server.Application.Validators;

public class AddDestinationAddressDtoValidator : AbstractValidator<AddDestinationAddressDto>
{
    public AddDestinationAddressDtoValidator()
    {
        RuleFor(x => x.Address)
            .NotNull()
            .SetValidator(new CreateAddressDtoValidator());
    }
}
