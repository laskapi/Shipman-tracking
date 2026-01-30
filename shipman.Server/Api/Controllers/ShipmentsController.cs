using Microsoft.AspNetCore.Mvc;
using shipman.Server.Application.Dtos;
using shipman.Server.Application.DTOs;
using shipman.Server.Application.Interfaces;
using shipman.Server.Application.Mappings;
using shipman.Server.Domain.Enums;
namespace shipman.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController : ControllerBase
{
    private readonly IShipmentService _service;
    public ShipmentsController(IShipmentService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ShipmentDetailsDto>> CreateShipment([FromBody] CreateShipmentDto dto)
    {
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
        var shipment = await _service.GetByIdAsync(id);

        if (shipment == null)
            return NotFound();

        return shipment.ToDetailsDto();
    }

    [HttpPost("{id}/events")]
    public async Task<ActionResult<ShipmentDetailsDto>> AddEvent(Guid id, AddShipmentEventDto dto)
    {
        try
        {
            var shipment = await _service.AddEventAsync(id, dto);

            if (shipment == null)
                return NotFound("Shipment not found.");

            return shipment.ToDetailsDto();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult<ShipmentDetailsDto>> Cancel(Guid id)
    {
        try
        {
            var dto = new AddShipmentEventDto
            {
                EventType = ShipmentEventType.Cancelled,
                Location = null,
                Description = "Shipment cancelled"
            };

            var shipment = await _service.AddEventAsync(id, dto);

            if (shipment == null)
                return NotFound("Shipment not found.");

            return shipment.ToDetailsDto();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ShipmentDetailsDto>> Update(Guid id, UpdateShipmentDto dto)
    {
        try
        {
            var shipment = await _service.UpdateShipmentAsync(id, dto);

            if (shipment == null)
                return NotFound("Shipment not found.");

            return shipment.ToDetailsDto();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteShipmentAsync(id);

        if (!deleted)
            return NotFound("Shipment not found.");

        return NoContent();
    }




}
