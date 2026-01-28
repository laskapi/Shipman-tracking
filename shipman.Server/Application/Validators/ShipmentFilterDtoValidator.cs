using FluentValidation;

namespace shipman.Server.Application.Validators;
public class ShipmentFilterDtoValidator : AbstractValidator<ShipmentFilterDto>
{
    public ShipmentFilterDtoValidator()
    {
        RuleFor(x => x.TrackingNumber)
            .MaximumLength(50)
            .When(x => x.TrackingNumber != null);

        RuleFor(x => x.Status)
            .IsInEnum()
            .When(x => x.Status != null);
    }
}
