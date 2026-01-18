using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using shipman.Server.Domain.Enums;

namespace shipman.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class MetadataController : ControllerBase
{
    [HttpGet("shipment-statuses")]
    public IActionResult GetShipmentStatuses()
    {
        var statuses = Enum.GetNames(typeof(ShipmentStatus));
        return Ok(statuses);
    }
}

