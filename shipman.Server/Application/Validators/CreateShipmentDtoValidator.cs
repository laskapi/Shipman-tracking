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

    }
}
