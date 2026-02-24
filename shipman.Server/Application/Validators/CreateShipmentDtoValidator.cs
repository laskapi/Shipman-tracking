namespace shipman.Server.Application.Validators;

using FluentValidation;

public class CreateShipmentDtoValidator : AbstractValidator<CreateShipmentDto>
{
    public CreateShipmentDtoValidator()
    {
        RuleFor(x => x.Sender).NotNull();
        RuleFor(x => x.Receiver).NotNull();

        RuleFor(x => x.Sender.Name).NotEmpty();
        RuleFor(x => x.Sender.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Sender.Phone).NotEmpty();
        RuleFor(x => x.Sender.Address).NotEmpty();

        RuleFor(x => x.Receiver.Name).NotEmpty();
        RuleFor(x => x.Receiver.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Receiver.Phone).NotEmpty();
        RuleFor(x => x.Receiver.Address).NotEmpty();

        RuleFor(x => x.Weight).GreaterThan(0);
        RuleFor(x => x.ServiceType).IsInEnum();
    }
}
