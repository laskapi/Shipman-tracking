using FluentValidation;
using shipman.Server.Application.Dtos.Contacts;

namespace shipman.Server.Application.Validators;

public class UpdateContactDtoValidator : AbstractValidator<UpdateContactDto>
{
    public UpdateContactDtoValidator()
    {
        When(x => x.Email is not null, () =>
        {
            RuleFor(x => x.Email!).EmailAddress();
        });

        When(x => x.PrimaryAddress is not null, () =>
        {
            RuleFor(x => x.PrimaryAddress!)
                .SetValidator(new CreateAddressDtoValidator());
        });
    }
}
