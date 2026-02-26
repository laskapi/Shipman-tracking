using Microsoft.AspNetCore.Mvc;
using shipman.Server.Application.Dtos;
using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Application.Interfaces;
using shipman.Server.Application.Mappings;
using shipman.Server.Domain.Enums;

namespace shipman.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly ILogger<ShipmentsController> _logger;
    private readonly IShipmentService _service;

    public ShipmentsController(
        ILogger<ShipmentsController> logger,
        IShipmentService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ShipmentDetailsDto>> CreateShipment([FromBody] CreateShipmentDto dto)
    {
        _logger.LogInformation("Received request to create shipment for {Receiver}", dto.Receiver.Name);

        var shipment = await _service.CreateShipmentAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = shipment.Id },
            shipment.ToDetailsDto()
        );
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ShipmentListItemDto>>> GetAll(
        int page = 1,
        int pageSize = 20,
        [FromQuery] ShipmentFilterDto? filter = null,
        string sortBy = "updatedAt",
        string direction = "desc")
    {
        _logger.LogInformation("Fetching shipments page {Page}", page);

        filter ??= new ShipmentFilterDto();
        var result = await _service.GetAllAsync(page, pageSize, filter, sortBy, direction);

        var dto = new PagedResultDto<ShipmentListItemDto>
        {
            Items = result.Items.Select(s => s.ToListItemDto()).ToList(),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        };

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShipmentDetailsDto>> GetById(Guid id)
    {
        _logger.LogInformation("Fetching shipment {ShipmentId}", id);

        var shipment = await _service.GetByIdAsync(id);
        return shipment.ToDetailsDto();
    }

    [HttpPost("{id}/events")]
    public async Task<ActionResult<ShipmentDetailsDto>> AddEvent(Guid id, AddShipmentEventDto dto)
    {
        _logger.LogInformation("Adding event {EventType} to shipment {ShipmentId}", dto.EventType, id);

        var shipment = await _service.AddEventAsync(id, dto);
        return shipment.ToDetailsDto();
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<ShipmentDetailsDto>> Cancel(Guid id)
    {
        _logger.LogInformation("Cancelling shipment {ShipmentId}", id);

        var dto = new AddShipmentEventDto(ShipmentEventType.Cancelled);
        var shipment = await _service.AddEventAsync(id, dto);

        return shipment.ToDetailsDto();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ShipmentDetailsDto>> Update(Guid id, UpdateShipmentDto dto)
    {
        _logger.LogInformation("Updating shipment {ShipmentId}", id);

        var shipment = await _service.UpdateShipmentAsync(id, dto);
        return shipment.ToDetailsDto();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Deleting shipment {ShipmentId}", id);

        await _service.DeleteShipmentAsync(id);
        return NoContent();
    }
}
