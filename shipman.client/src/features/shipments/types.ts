export interface ShipmentEvent {
    timestamp: string;
    eventType: string;
    location: string;
    description: string;
}

export interface ShipmentDetails {
    id: string;
    trackingNumber: string;

    sender: ContactDto;
    receiver: ContactDto;

    originCoordinates: CoordinatesDto;
    destinationCoordinates: CoordinatesDto;

    weight: number;
    serviceType: string;
    status: string;

    origin: string;
    destination: string;

    createdAt: string;
    updatedAt: string;
    estimatedDelivery: string | null;

    events: ShipmentEvent[];
}

export interface ContactDto {
    name: string;
    email: string;
    phone: string;
}

export interface CoordinatesDto {
    lat: number;
    lng: number;
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

import type { GridSortModel } from "@mui/x-data-grid"

export interface ShipmentsToolbarController {
    statuses: MetadataOptionDto[]
    status: string
    search: string
    setStatus: (value: string) => void
    setSearch: (value: string) => void
    clear: () => void
    refresh: () => void
}

export interface ShipmentsController {
    data: { items: ShipmentListItem[]; totalCount: number } | undefined
    statuses: MetadataOptionDto[]
    status: string
    search: string
    sortModel: GridSortModel
    page: number
    pageSize: number
    isLoading: boolean
    isError: boolean

    setStatus: (value: string) => void
    setSearch: (value: string) => void
    setSortModel: (model: GridSortModel) => void
    setPage: (page: number) => void
    setPageSize: (size: number) => void
    refetch: () => void

    handleRowClick: (params: { id: string | number }) => void

    toolbar: ShipmentsToolbarController
}
