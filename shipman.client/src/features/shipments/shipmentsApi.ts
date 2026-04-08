import { api } from '@/services/api';
import type {
    ShipmentListItem,
    ShipmentDetails,
    ShipmentsQueryParams,
    PagedResult,
    MetadataOptionDto,
    ContactListItemDto,
    ContactDetailsDto
} from './types';

import type { CreateShipmentDto } from './create/createShipmentSchema';
import type { EditShipmentDto } from './edit/editShipmentSchema';
import type { CreateContactFormValues } from '@/features/contacts/contactSchema';
import type { CreateAddressFormValues } from '@/features/addresses/addressSchema';

export const shipmentsApi = api.injectEndpoints({
    endpoints: builder => ({
        getShipments: builder.query<PagedResult<ShipmentListItem>, ShipmentsQueryParams>({
            query: params => ({ url: 'shipments', params }),
            providesTags: result =>
                result
                    ? [
                          ...result.items.map(s => ({
                              type: 'Shipment' as const,
                              id: s.id
                          })),
                          { type: 'Shipment', id: 'LIST' }
                      ]
                    : [{ type: 'Shipment', id: 'LIST' }]
        }),

        getShipmentById: builder.query<ShipmentDetails, string>({
            query: id => `shipments/${id}`,
            providesTags: (_result, _error, id) => [{ type: 'Shipment', id }]
        }),

        createShipment: builder.mutation<ShipmentDetails, CreateShipmentDto>({
            query: body => ({
                url: 'shipments',
                method: 'POST',
                body
            }),
            invalidatesTags: [{ type: 'Shipment', id: 'LIST' }]
        }),

        updateShipment: builder.mutation<
            ShipmentDetails,
            { id: string; data: EditShipmentDto }
        >({
            query: ({ id, data }) => ({
                url: `shipments/${id}`,
                method: 'PUT',
                body: data
            }),
            invalidatesTags: (_result, _error, { id }) => [
                { type: 'Shipment', id },
                { type: 'Shipment', id: 'LIST' }
            ]
        }),

        addShipmentEvent: builder.mutation<
            ShipmentDetails,
            { id: string; eventType: string }
        >({
            query: ({ id, eventType }) => ({
                url: `shipments/${id}/events`,
                method: 'POST',
                body: { eventType }
            }),
            invalidatesTags: (_result, _error, { id }) => [
                { type: 'Shipment', id },
                { type: 'Shipment', id: 'LIST' }
            ]
        }),

        deleteShipment: builder.mutation<void, string>({
            query: id => ({
                url: `shipments/${id}`,
                method: 'DELETE'
            }),
            invalidatesTags: (_result, _error, id) => [
                { type: 'Shipment', id },
                { type: 'Shipment', id: 'LIST' }
            ]
        }),

        getShipmentStatuses: builder.query<MetadataOptionDto[], void>({
            query: () => 'metadata/shipment-statuses'
        }),

        getShipmentEventTypes: builder.query<MetadataOptionDto[], void>({
            query: () => 'metadata/shipment-event-types'
        }),

        getContacts: builder.query<ContactListItemDto[], void>({
            query: () => 'contacts',
            providesTags: [{ type: 'Contact', id: 'LIST' }]
        }),

        getContactById: builder.query<ContactDetailsDto, string>({
            query: id => `contacts/${id}`,
            providesTags: (_result, _error, id) => [{ type: 'Contact', id }]
        }),

        createContact: builder.mutation<ContactDetailsDto, CreateContactFormValues>({
            query: body => ({
                url: 'contacts',
                method: 'POST',
                body: {
                    name: body.name,
                    email: body.email,
                    phone: body.phone,
                    primaryAddress: {
                        street: body.primaryAddress.street,
                        houseNumber: body.primaryAddress.houseNumber,
                        apartmentNumber:
                            body.primaryAddress.apartmentNumber?.trim() || null,
                        city: body.primaryAddress.city,
                        postalCode: body.primaryAddress.postalCode,
                        country: body.primaryAddress.country
                    }
                }
            }),
            invalidatesTags: [{ type: 'Contact', id: 'LIST' }]
        }),

        addContactDestinationAddress: builder.mutation<
            ContactDetailsDto,
            { contactId: string; address: CreateAddressFormValues }
        >({
            query: ({ contactId, address }) => ({
                url: `contacts/${contactId}/addresses`,
                method: 'POST',
                body: {
                    address: {
                        street: address.street,
                        houseNumber: address.houseNumber,
                        apartmentNumber: address.apartmentNumber?.trim() || null,
                        city: address.city,
                        postalCode: address.postalCode,
                        country: address.country
                    }
                }
            }),
            invalidatesTags: (_result, _error, { contactId }) => [
                { type: 'Contact', id: contactId }
            ]
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
    useGetContactByIdQuery,
    useCreateContactMutation,
    useAddContactDestinationAddressMutation
} = shipmentsApi;
