using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

namespace shipman.Tests.Unit.Domain
{
    internal class DomainTestFactory
    {

        public static Shipment CreateShipment()
        {
            return new Shipment
            {
                Id = Guid.NewGuid(),
                Origin = "A",
                Destination = "B",
                Sender = "Sender",
                Receiver = "Receiver",
                Weight = 1,
                ServiceType = ServiceType.Standard
            };
        }

        public static ShipmentEvent CreateEvent(ShipmentEventType type)
        {
            return new ShipmentEvent
            {
                Id = Guid.NewGuid(),
                EventType = type,
                Timestamp = DateTime.UtcNow,
                Location = "A",
                Description = type.ToString()
            };
        }
    }
}
