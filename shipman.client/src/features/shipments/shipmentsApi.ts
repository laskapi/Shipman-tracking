import { api } from "@/services/api";
import type { CreateShipmentRequest, MetadataOptionDto, PagedResult, Shipment, ShipmentDetails, ShipmentListItem, ShipmentsQueryParams } from "./types";


export const shipmentsApi = api.injectEndpoints({
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
            }),
            invalidatesTags: ["Shipment"]
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
} = shipmentsApi
