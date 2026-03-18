using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using shipman.Server.Application.Dtos;
using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Application.Interfaces;
using shipman.Server.Domain.Enums;

namespace shipman.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly ILogger<ShipmentsController> _logger;
    private readonly IShipmentService _service;
    private readonly IMapper _mapper;

    public ShipmentsController(
        ILogger<ShipmentsController> logger,
        IShipmentService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<ShipmentDetailsDto>> CreateShipment([FromBody] ShipmentCreateDto dto)
    {
        _logger.LogInformation("Received request to create shipment for receiver {ReceiverId}", dto.ReceiverId);

        var shipment = await _service.CreateShipmentAsync(dto);
        var result = _mapper.Map<ShipmentDetailsDto>(shipment);

        return CreatedAtAction(nameof(GetById), new { id = shipment.Id }, result);
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

        var dtoItems = result.Items
            .Select(s => _mapper.Map<ShipmentListItemDto>(s))
            .ToList();

        return Ok(new PagedResultDto<ShipmentListItemDto>
        {
            Items = dtoItems,
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShipmentDetailsDto>> GetById(Guid id)
    {
        _logger.LogInformation("Fetching shipment {ShipmentId}", id);

        var shipment = await _service.GetByIdAsync(id);

        return _mapper.Map<ShipmentDetailsDto>(shipment);
    }

    [HttpPost("{id}/events")]
    public async Task<ActionResult<ShipmentDetailsDto>> AddEvent(Guid id, ShipmentEventCreateDto dto)
    {
        _logger.LogInformation("Adding event {EventType} to shipment {ShipmentId}", dto.EventType, id);

        var shipment = await _service.AddEventAsync(id, dto);

        return _mapper.Map<ShipmentDetailsDto>(shipment);
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<ShipmentDetailsDto>> Cancel(Guid id)
    {
        _logger.LogInformation("Cancelling shipment {ShipmentId}", id);

        var dto = new ShipmentEventCreateDto
        {
            EventType = ShipmentEventType.Cancelled.ToString()
        };

        var shipment = await _service.AddEventAsync(id, dto);

        return _mapper.Map<ShipmentDetailsDto>(shipment);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ShipmentDetailsDto>> Update(Guid id, UpdateShipmentDto dto)
    {
        _logger.LogInformation("Updating shipment {ShipmentId}", id);

        var shipment = await _service.UpdateShipmentAsync(id, dto);

        return _mapper.Map<ShipmentDetailsDto>(shipment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        _logger.LogInformation("Deleting shipment {ShipmentId}", id);

        await _service.DeleteShipmentAsync(id);

        return NoContent();
    }
}
