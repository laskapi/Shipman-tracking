using FluentValidation;
using shipman.Server.Application.Dtos.Shipments;

namespace shipman.Server.Application.Validators;

public class CreateShipmentDtoValidator : AbstractValidator<ShipmentCreateDto>
{
    public CreateShipmentDtoValidator()
    {
        RuleFor(x => x.SenderId)
            .NotEmpty();

        RuleFor(x => x.ReceiverId)
            .NotEmpty();

        RuleFor(x => x.Weight)
            .GreaterThan(0);

        RuleFor(x => x.ServiceType)
            .NotEmpty();

        // Exactly one of the two must be provided
        RuleFor(x => x)
            .Must(x =>
                (x.DestinationAddressId.HasValue && x.DestinationAddress is null) ||
                (!x.DestinationAddressId.HasValue && x.DestinationAddress is not null)
            )
            .WithMessage("Provide either DestinationAddressId OR DestinationAddress, not both.");

        // Validate the new address if provided
        When(x => x.DestinationAddress is not null, () =>
        {
            RuleFor(x => x.DestinationAddress!)
                .SetValidator(new AddressDtoValidator());
        });
    }
}
