using shipman.Server.Application.Dtos;
using shipman.Server.Domain.Enums;

namespace shipman.Tests.TestUtils;

public static class DtoFactory
{
    public static CreateShipmentDto CreateShipment(
        string sender = "Alice",
        string receiverName = "Bob",
        string receiverEmail = "bob@example.com",
        string receiverPhone = "+48 600 000 000",
        string origin = "Berlin",
        string destination = "Paris",
        decimal weight = 2.5m,
        ServiceType serviceType = ServiceType.Standard)
    {
        return new CreateShipmentDto
        {
            Sender = sender,
            ReceiverName = receiverName,
            ReceiverEmail = receiverEmail,
            ReceiverPhone = receiverPhone,
            Origin = origin,
            Destination = destination,
            Weight = weight,
            ServiceType = serviceType
        };
    }

    public static AddShipmentEventDto CreateEventDto(
  ShipmentEventType type,
  string? location = "A",
  string? description = null)
    {
        return new AddShipmentEventDto
        {
            EventType = type,
            Location = location,
            Description = description ?? type.ToString()
        };
    }
}
