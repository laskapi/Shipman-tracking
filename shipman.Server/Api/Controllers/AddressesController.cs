using Microsoft.AspNetCore.Mvc;
using shipman.Server.Application.Services.Addresses;

namespace shipman.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressesController : ControllerBase
{
    private readonly AddressService _addressService;

    public AddressesController(AddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAddress(Guid id)
    {
        await _addressService.DeleteAddressAsync(id);
        return NoContent();
    }
}
