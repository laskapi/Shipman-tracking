import type { PagedResult, Shipment, ShipmentDetails, ShipmentListItem, ShipmentsQueryParams, MetadataOptionDto } from "./types"
import { api } from "@/services/api"
export interface CreateShipmentRequest {
    trackingNumber: string
    sender: string
    receiver: string
}
export interface UpdateShipmentStatusRequest {
    id: string
    status: string
}

export const shipmentApi = api.injectEndpoints({
    endpoints: builder => ({
        getShipments: builder.query<PagedResult<ShipmentListItem>, ShipmentsQueryParams>({
            query: (params) => ({ url: "shipments", params })
        }),

        getShipmentById: builder.query<ShipmentDetails, string>({
            query: id => `shipments/${id}`,
            providesTags: (result, error, id) => [{ type: "Shipment", id }]
        }),

        createShipment: builder.mutation<Shipment, CreateShipmentRequest>({
            query: body => ({
                url: "shipments",
                method: "POST",
                body
            })
        }),

        addShipmentEvent: builder.mutation<ShipmentDetails, {
            id: string;
            eventType: string;
        }>({
            query: ({ id, eventType }) => ({
                url: `shipments/${id}/events`,
                method: "POST",
                body: { eventType }
            }),
            invalidatesTags: (result, error, { id }) => [{ type: "Shipment", id }]
        }),

        getShipmentStatuses: builder.query<MetadataOptionDto[], void>({
            query: () => "metadata/shipment-statuses"
        }),

        getShipmentEventTypes: builder.query<MetadataOptionDto[], void>({
            query: () => "metadata/shipment-event-types"
        })

    })
})

export const {
    useGetShipmentsQuery,
    useGetShipmentByIdQuery,
    useCreateShipmentMutation,
    useAddShipmentEventMutation,
    useGetShipmentStatusesQuery,
    useGetShipmentEventTypesQuery
} = shipmentApi
