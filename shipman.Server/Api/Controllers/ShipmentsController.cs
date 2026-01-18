using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Create(CreateShipmentDto dto)
    {
        var shipment = await _service.CreateAsync(dto);
        return Ok(shipment);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var shipments = await _service.GetAllAsync();
        return Ok(shipments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var shipment = await _service.GetByIdAsync(id);
        return shipment is null ? NotFound() : Ok(shipment);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, UpdateShipmentStatusDto dto)
    {
        var updated = await _service.UpdateStatusAsync(id, dto.Status);
        return Ok(updated);
    }
}
