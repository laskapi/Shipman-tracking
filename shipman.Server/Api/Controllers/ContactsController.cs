using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using shipman.Server.Application.Dtos.Contacts;
using shipman.Server.Application.Services.Contacts;

namespace shipman.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ContactService _service;
    private readonly IMapper _mapper;

    public ContactsController(ContactService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<ContactDetailsDto>> Create(CreateContactDto dto)
    {
        var contact = await _service.CreateAsync(dto);
        return _mapper.Map<ContactDetailsDto>(contact);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContactDetailsDto>> GetById(Guid id)
    {
        var contact = await _service.GetByIdAsync(id);
        return _mapper.Map<ContactDetailsDto>(contact);
    }

    [HttpGet]
    public async Task<ActionResult<List<ContactListItemDto>>> Search([FromQuery] string? search)
    {
        var contacts = await _service.SearchAsync(search);
        return contacts.Select(_mapper.Map<ContactListItemDto>).ToList();
    }

    [HttpPost("{id}/addresses")]
    public async Task<ActionResult<ContactDetailsDto>> AddDestinationAddress(Guid id, AddDestinationAddressDto dto)
    {
        var contact = await _service.AddDestinationAddressAsync(id, dto.Address);
        return _mapper.Map<ContactDetailsDto>(contact);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ContactDetailsDto>> Update(Guid id, UpdateContactDto dto)
    {
        var contact = await _service.UpdateAsync(id, dto);
        return _mapper.Map<ContactDetailsDto>(contact);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
