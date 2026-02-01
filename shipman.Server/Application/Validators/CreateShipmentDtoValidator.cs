namespace shipman.Server.Application.Validators;
using FluentValidation;

public class CreateShipmentDtoValidator : AbstractValidator<CreateShipmentDto>
{
    public CreateShipmentDtoValidator()
    {
        RuleFor(x => x.Sender).NotEmpty();
        RuleFor(x => x.ReceiverName).NotEmpty();
        RuleFor(x => x.ReceiverEmail).NotEmpty().EmailAddress();
        RuleFor(x => x.ReceiverPhone).NotEmpty();
        RuleFor(x => x.Origin).NotEmpty();
        RuleFor(x => x.Destination).NotEmpty();
        RuleFor(x => x.Weight).GreaterThan(0);
        RuleFor(x => x.ServiceType).IsInEnum();
    }
}
