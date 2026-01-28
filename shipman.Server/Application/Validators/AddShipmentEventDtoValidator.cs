namespace shipman.Server.Application.Validators;
using FluentValidation;

public class AddShipmentEventDtoValidator : AbstractValidator<AddShipmentEventDto>
{
    public AddShipmentEventDtoValidator()
    {
        RuleFor(x => x.EventType).IsInEnum();
        RuleFor(x => x.Location).NotEmpty().When(x => x.Location != null);
        RuleFor(x => x.Description).NotEmpty().When(x => x.Description != null);
    }
}
