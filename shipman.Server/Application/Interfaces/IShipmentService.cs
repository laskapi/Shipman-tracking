using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

public interface IShipmentService
{
    Task<Shipment> CreateShipmentAsync(CreateShipmentDto dto);
    Task<List<Shipment>> GetAllAsync();
    Task<Shipment?> GetByIdAsync(Guid id);
    Task<Shipment?> UpdateStatusAsync(Guid id, UpdateShipmentStatusDto dto);
}
