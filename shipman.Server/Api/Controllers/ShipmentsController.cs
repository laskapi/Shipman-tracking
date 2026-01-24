using Microsoft.AspNetCore.Mvc;
using shipman.Server.Application.Mappings;

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
    public async Task<IActionResult> GetAll()
    {
        var shipments = await _service.GetAllAsync();
        return Ok(shipments);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ShipmentDetailsDto>> GetById(Guid id)
    {
        var shipment = await _service.GetByIdAsync(id);

        if (shipment == null)
            return NotFound();

        return shipment.ToDetailsDto();
    }
    
    [HttpPatch("{id}/status")]
    public async Task<ActionResult<ShipmentDetailsDto>> UpdateStatus(Guid id, UpdateShipmentStatusDto dto)
    {
        var shipment = await _service.UpdateStatusAsync(id, dto);
        if (shipment == null)
            return NotFound();
        return shipment.ToDetailsDto();
    }


}
