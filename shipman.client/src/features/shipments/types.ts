export interface ShipmentEvent {
    timestamp: string;
    eventType: string;
    location: string;
    description: string;
}


export interface ShipmentDetails {
    id: string;
    trackingNumber: string;
    sender: string;
    receiverName: string;
    receiverEmail: string;
    receiverPhone: string;
    origin: string;
    destination: string;
    weight: number;
    serviceType: string;
    status: string;
    createdAt: string;
    updatedAt: string;
    estimatedDelivery: string | null;
    events: ShipmentEvent[];
}
export interface ShipmentListItem {
    id: string
    trackingNumber: string
    sender: string
    receiver: string
    origin: string
    destination: string
    status: string
    updatedAt: string
}


export interface PagedResult<T> {
    items: T[]
    page: number
    pageSize: number
    totalCount: number
    totalPages: number
}

export interface ShipmentsQueryParams {
    page?: number
    pageSize?: number
    trackingNumber?: string
    status?: string
    sortBy?: string
    direction?: "asc" | "desc"
}
export interface Shipment {
    id: string
    trackingNumber: string
    sender: string
    receiver: string
    status: string
    createdAt: string
}
export interface MetadataOptionDto {
    value: string
    label: string
}
