import { api } from "@/services/api";
import type {
    MetadataOptionDto,
    PagedResult,
    Shipment,
    ShipmentDetails,
    ShipmentsQueryParams
} from "./types";

import type { CreateShipmentDto } from "./create/createShipmentSchema";
import type { EditShipmentDto } from "./edit/editShipmentSchema";

export const shipmentsApi = api.injectEndpoints({
    endpoints: builder => ({

        // LIST
        getShipments: builder.query<PagedResult<Shipment>, ShipmentsQueryParams>({
            query: params => ({ url: "shipments", params }),
            providesTags: result =>
                result
                    ? [
                        ...result.items.map(s => ({ type: "Shipment" as const, id: s.id })),
                        { type: "Shipment", id: "LIST" }
                    ]
                    : [{ type: "Shipment", id: "LIST" }]
        }),

        // DETAILS
        getShipmentById: builder.query<ShipmentDetails, string>({
            query: id => `shipments/${id}`,
            providesTags: (_result, _error, id) => [{ type: "Shipment", id }]
        }),

        // CREATE
        createShipment: builder.mutation<ShipmentDetails, CreateShipmentDto>({
            query: body => ({
                url: "shipments",
                method: "POST",
                body
            }),
            invalidatesTags: [{ type: "Shipment", id: "LIST" }]
        }),

        // UPDATE (partial)
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

        // ADD EVENT
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

        // DELETE
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

        // METADATA
        getShipmentStatuses: builder.query<MetadataOptionDto[], void>({
            query: () => "metadata/shipment-statuses"
        }),

        getShipmentEventTypes: builder.query<MetadataOptionDto[], void>({
            query: () => "metadata/shipment-event-types"
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
    useGetShipmentStatusesQuery
} = shipmentsApi;
