using FluentValidation;
using shipman.Server.Application.Dtos.Shipments;

namespace shipman.Server.Application.Validators;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty();

        RuleFor(x => x.HouseNumber)
            .NotEmpty();

        RuleFor(x => x.City)
            .NotEmpty();

        RuleFor(x => x.PostalCode)
            .NotEmpty();

        RuleFor(x => x.Country)
            .NotEmpty();

        RuleFor(x => x.Latitude)
            .NotNull();

        RuleFor(x => x.Longitude)
            .NotNull();
    }
}
