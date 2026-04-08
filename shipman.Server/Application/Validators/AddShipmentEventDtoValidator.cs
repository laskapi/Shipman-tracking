namespace shipman.Server.Application.Validators;

using FluentValidation;
using shipman.Server.Domain.Enums;

public class AddShipmentEventDtoValidator : AbstractValidator<ShipmentEventCreateDto>
{
    public AddShipmentEventDtoValidator()
    {
        RuleFor(x => x.EventType)
            .NotEmpty()
            .Must(et =>
                Enum.TryParse<ShipmentEventType>(et, ignoreCase: true, out var v) &&
                Enum.IsDefined(typeof(ShipmentEventType), v))
            .WithMessage("Invalid event type");
    }
}
