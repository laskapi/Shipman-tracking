namespace shipman.Server.Application.Validators;

using FluentValidation;

public class AddShipmentEventDtoValidator : AbstractValidator<AddShipmentEventDto>
{
    public AddShipmentEventDtoValidator()
    {
        RuleFor(x => x.EventType).IsInEnum();
    }
}
