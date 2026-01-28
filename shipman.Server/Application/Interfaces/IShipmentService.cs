using shipman.Server.Application.Dtos;
using shipman.Server.Domain.Entities;
namespace shipman.Server.Application.Interfaces;

public interface IShipmentService
{
    Task<Shipment> CreateShipmentAsync(CreateShipmentDto dto);
    Task<PagedResultDto<Shipment>> GetAllAsync(int page, int pageSize, ShipmentFilterDto filter, string sortBy,
      string direction);
    Task<Shipment?> GetByIdAsync(Guid id);
    Task<Shipment?> UpdateStatusAsync(Guid id, UpdateShipmentStatusDto dto);
}
