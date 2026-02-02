using Microsoft.AspNetCore.Mvc;
using shipman.Server.Domain.Enums;

namespace shipman.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetadataController : ControllerBase
{
    private readonly ILogger<MetadataController> _logger;

    public MetadataController(ILogger<MetadataController> logger)
    {
        _logger = logger;
    }

    [HttpGet("shipment-statuses")]
    public IActionResult GetShipmentStatuses()
    {
        _logger.LogInformation("Fetching shipment statuses metadata");

        var statuses = Enum.GetValues(typeof(ShipmentStatus))
            .Cast<ShipmentStatus>()
            .Select(s => new
            {
                value = s.ToString(),
                label = s.ToString()
            });

        return Ok(statuses);
    }

    [HttpGet("shipment-event-types")]
    public IActionResult GetShipmentEventTypes()
    {
        _logger.LogInformation("Fetching shipment event types metadata");

        var events = Enum.GetValues(typeof(ShipmentEventType))
            .Cast<ShipmentEventType>()
            .Select(e => new
            {
                value = e.ToString(),
                label = e.ToString()
            });

        return Ok(events);
    }

    [HttpGet("service-types")]
    public IActionResult GetServiceTypes()
    {
        _logger.LogInformation("Fetching service types metadata");

        var types = Enum.GetValues(typeof(ServiceType))
            .Cast<ServiceType>()
            .Select(t => new
            {
                value = t.ToString(),
                label = t.ToString()
            });

        return Ok(types);
    }
}
