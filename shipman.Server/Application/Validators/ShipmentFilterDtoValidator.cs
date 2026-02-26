using FluentValidation;
using shipman.Server.Domain.Enums;

namespace shipman.Server.Application.Validators;

public class ShipmentFilterDtoValidator : AbstractValidator<ShipmentFilterDto>
{
    public ShipmentFilterDtoValidator()
    {
        RuleFor(x => x.TrackingNumber)
            .MaximumLength(50)
            .When(x => x.TrackingNumber != null);

        RuleFor(x => x.Status)
     .Must(x => x == null || Enum.IsDefined(typeof(ShipmentStatus), x))
     .WithMessage("Invalid shipment status");

    }
}
