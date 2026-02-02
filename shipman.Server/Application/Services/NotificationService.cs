using shipman.Server.Application.Interfaces;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMailSender _mail;

        public NotificationService(IMailSender mail)
        {
            _mail = mail;
        }

        public Task ShipmentCancelledAsync(Shipment shipment)
        {
            return _mail.SendAsync(
                shipment.Receiver.Email,
                "Shipment cancelled",
                $"Your shipment {shipment.TrackingNumber} has been cancelled"
                );

        }

        public Task ShipmentCreatedAsync(Shipment shipment)
        {
            return _mail.SendAsync(
                shipment.Receiver.Email,
                "Shipment created",
                $"Your shipment {shipment.TrackingNumber} has been created."
                );
        }

        public Task ShipmentDeliveredAsync(Shipment shipment)
        {
            return _mail.SendAsync(
                shipment.Receiver.Email,
                "Shipment delivered",
                $"Your shipment {shipment.TrackingNumber} has been delivered."
                );
        }
    }
}
