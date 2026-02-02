using shipman.Server.Domain.Entities;

namespace shipman.Server.Application.Interfaces
{
    public interface INotificationService
    {
        Task ShipmentCreatedAsync(Shipment shipment);
        Task ShipmentDeliveredAsync(Shipment shipment);
        Task ShipmentCancelledAsync(Shipment shipment);
    }
}
