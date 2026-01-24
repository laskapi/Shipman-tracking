import type { ShipmentDetails } from "../types/shipment"
import { api } from "./api"

export interface Shipment {
    id: string
    trackingNumber: string
    sender: string
    receiver: string
    status: string
    createdAt: string
}

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
        getShipments: builder.query<Shipment[], void>({
            query: () => "shipments"
        }),

        getShipmentById: builder.query<ShipmentDetails, string>({
            query: id => `shipments/${id}`
        }),

        createShipment: builder.mutation<Shipment, CreateShipmentRequest>({
            query: body => ({
                url: "shipments",
                method: "POST",
                body
            })
        }),

        updateShipmentStatus: builder.mutation<Shipment, UpdateShipmentStatusRequest>({
            query: ({ id, status }) => ({
                url: `shipments/${id}/status`,
                method: "PUT",
                body: { status }
            })
        }),
        getShipmentStatuses: builder.query<string[], void>({
            query: ()=>"metadata/shipment-statuses"
        })
    })
})

export const {
    useGetShipmentsQuery,
    useGetShipmentByIdQuery,
    useCreateShipmentMutation,
    useUpdateShipmentStatusMutation,
    useGetShipmentStatusesQuery
} = shipmentApi
