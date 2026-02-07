using Microsoft.AspNetCore.Mvc;
using shipman.Server.Application.Dtos;
using shipman.Server.Domain.Enums;
using System.Text.RegularExpressions;

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
    public ActionResult<IEnumerable<MetadataOptionDto>> GetShipmentStatuses()
    {
        _logger.LogInformation("Fetching status types metadata");
        var statuses = Enum.GetValues<ShipmentStatus>()
            .Select(s => new MetadataOptionDto(
                s.ToString(),
                s.ToString()
            ));

        return Ok(statuses);
    }

    [HttpGet("shipment-event-types")]
    public ActionResult<IEnumerable<MetadataOptionDto>> GetShipmentEventTypes()
    {
        _logger.LogInformation("Fetching event types metadata");
        var events = Enum.GetValues<ShipmentEventType>()
            .Select(e => new MetadataOptionDto(
                e.ToString(),
                ToLabel(e.ToString())
            ));

        return Ok(events);
    }



    [HttpGet("service-types")]
    public ActionResult<IEnumerable<MetadataOptionDto>> GetServiceTypes()
    {
        _logger.LogInformation("Fetching service types metadata");

        var types = Enum.GetValues<ServiceType>()
            .Select(t => new MetadataOptionDto(
                t.ToString(),
                t.ToString()
            ));

        return Ok(types);
    }

    private string ToLabel(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var spaced = Regex.Replace(input, "([A-Z])", " $1").Trim();

        return char.ToUpper(spaced[0]) + spaced.Substring(1).ToLower();
    }


}
