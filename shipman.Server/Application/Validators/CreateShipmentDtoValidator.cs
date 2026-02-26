namespace shipman.Server.Application.Validators;

using FluentValidation;
public class CreateShipmentDtoValidator : AbstractValidator<CreateShipmentDto>
{
    public CreateShipmentDtoValidator()
    {
        RuleFor(x => x.Sender).SetValidator(new ContactDtoValidator());
        RuleFor(x => x.Receiver).SetValidator(new ContactDtoValidator());

        RuleFor(x => x.Weight).GreaterThan(0);
        RuleFor(x => x.ServiceType).IsInEnum();
    }
}
