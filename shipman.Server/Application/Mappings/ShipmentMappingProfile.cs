using AutoMapper;
using shipman.Server.Application.Dtos.Contacts;
using shipman.Server.Application.Dtos.Events;
using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Application.Mappings;

public class ShipmentMappingProfile : Profile
{
    public ShipmentMappingProfile()
    {
        CreateMap<Address, AddressDto>();

        CreateMap<Contact, ContactDto>()
            .ForMember(d => d.Address, opt => opt.MapFrom(s => s.PrimaryAddress));

        CreateMap<ShipmentEvent, ShipmentEventDto>()
            .ForMember(d => d.EventType, opt => opt.MapFrom(s => s.EventType.ToString()));

        CreateMap<Shipment, ShipmentDetailsDto>()
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.DestinationAddress, opt => opt.MapFrom(s => s.DestinationAddress));

        CreateMap<Shipment, ShipmentListItemDto>()
            .ForMember(d => d.Sender, opt => opt.MapFrom(s => s.Sender.Name))
            .ForMember(d => d.Receiver, opt => opt.MapFrom(s => s.Receiver.Name))
            .ForMember(d => d.Destination, opt => opt.MapFrom(s => s.DestinationAddress.City))
            .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()));

        CreateMap<Contact, ContactListItemDto>();

        CreateMap<Contact, ContactDetailsDto>()
            .ForMember(d => d.PrimaryAddress, opt => opt.MapFrom(s => s.PrimaryAddress))
            .ForMember(d => d.DestinationAddresses, opt => opt.MapFrom(s => s.DestinationAddresses.Select(x => x.Address)));

    }
}
