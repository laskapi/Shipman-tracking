import { api } from "@/services/api";
import type {
    ShipmentListItem,
    ShipmentDetails,
    ShipmentsQueryParams,
    PagedResult,
    MetadataOptionDto,
    ContactDto,
    AddressDto
} from "./types";

import type { CreateShipmentDto } from "./create/createShipmentSchema";
import type { EditShipmentDto } from "./edit/editShipmentSchema";

export const shipmentsApi = api.injectEndpoints({
    endpoints: builder => ({

        // List shipments
        getShipments: builder.query<PagedResult<ShipmentListItem>, ShipmentsQueryParams>({
            query: params => ({ url: "shipments", params }),
            providesTags: result =>
                result
                    ? [
                        ...result.items.map(s => ({ type: "Shipment" as const, id: s.id })),
                        { type: "Shipment", id: "LIST" }
                    ]
                    : [{ type: "Shipment", id: "LIST" }]
        }),

        // Shipment details
        getShipmentById: builder.query<ShipmentDetails, string>({
            query: id => `shipments/${id}`,
            providesTags: (_result, _error, id) => [{ type: "Shipment", id }]
        }),

        // Create shipment
        createShipment: builder.mutation<ShipmentDetails, CreateShipmentDto>({
            query: body => ({
                url: "shipments",
                method: "POST",
                body
            }),
            invalidatesTags: [{ type: "Shipment", id: "LIST" }]
        }),

        // Update shipment
        updateShipment: builder.mutation<
            ShipmentDetails,
            { id: string; data: EditShipmentDto }
        >({
            query: ({ id, data }) => ({
                url: `shipments/${id}`,
                method: "PUT",
                body: data
            }),
            invalidatesTags: (_result, _error, { id }) => [
                { type: "Shipment", id },
                { type: "Shipment", id: "LIST" }
            ]
        }),

        // Add shipment event
        addShipmentEvent: builder.mutation<
            ShipmentDetails,
            { id: string; eventType: string }
        >({
            query: ({ id, eventType }) => ({
                url: `shipments/${id}/events`,
                method: "POST",
                body: { eventType }
            }),
            invalidatesTags: (_result, _error, { id }) => [
                { type: "Shipment", id },
                { type: "Shipment", id: "LIST" }
            ]
        }),

        // Delete shipment
        deleteShipment: builder.mutation<void, string>({
            query: id => ({
                url: `shipments/${id}`,
                method: "DELETE"
            }),
            invalidatesTags: (_result, _error, id) => [
                { type: "Shipment", id },
                { type: "Shipment", id: "LIST" }
            ]
        }),

        // Shipment statuses
        getShipmentStatuses: builder.query<MetadataOptionDto[], void>({
            query: () => "metadata/shipment-statuses"
        }),

        // Shipment event types
        getShipmentEventTypes: builder.query<MetadataOptionDto[], void>({
            query: () => "metadata/shipment-event-types"
        }),

        // Contacts
        getContacts: builder.query<ContactDto[], void>({
            query: () => "contacts"
        }),

        // Addresses
        getAddresses: builder.query<AddressDto[], void>({
            query: () => "addresses"
        }),

        // Create contact
        createContact: builder.mutation<ContactDto, Partial<ContactDto>>({
            query: body => ({
                url: "contacts",
                method: "POST",
                body
            })
        }),

        // Create address
        createAddress: builder.mutation<AddressDto, Partial<AddressDto>>({
            query: body => ({
                url: "addresses",
                method: "POST",
                body
            })
        })
    })
});

export const {
    useGetShipmentsQuery,
    useGetShipmentByIdQuery,
    useCreateShipmentMutation,
    useUpdateShipmentMutation,
    useAddShipmentEventMutation,
    useDeleteShipmentMutation,
    useGetShipmentEventTypesQuery,
    useGetShipmentStatusesQuery,
    useGetContactsQuery,
    useGetAddressesQuery,
    useCreateContactMutation,
    useCreateAddressMutation
} = shipmentsApi;
