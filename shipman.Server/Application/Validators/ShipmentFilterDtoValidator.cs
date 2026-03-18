using FluentValidation;
using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Domain.Enums;

namespace shipman.Server.Application.Validators;

public class ShipmentFilterDtoValidator : AbstractValidator<ShipmentFilterDto>
{
    public ShipmentFilterDtoValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(50)
            .When(x => x.Search is not null);

        RuleFor(x => x.Status)
            .Must(x => x == null || Enum.IsDefined(typeof(ShipmentStatus), x))
            .WithMessage("Invalid shipment status");
    }
}
