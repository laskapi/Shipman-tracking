namespace shipman.Server.Application.Validators;

using FluentValidation;

public class AddShipmentEventDtoValidator : AbstractValidator<ShipmentEventCreateDto>
{
    public AddShipmentEventDtoValidator()
    {
        RuleFor(x => x.EventType)
            .NotNull()
            .IsInEnum();
    }
}
