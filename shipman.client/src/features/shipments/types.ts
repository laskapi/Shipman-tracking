import type { GridSortModel } from '@mui/x-data-grid';

// Shipment event shown in the timeline
export interface ShipmentEvent
{
    id: string;
    timestamp: string;
    eventType: string;
    location: string;
    description: string;
}

// Supported service levels
export type ServiceType = 'Standard' | 'Express' | 'Freight';

/** Party on shipment details (matches API nested sender/receiver) */
export interface ShipmentPartyDto
{
    id: string;
    name: string;
    email: string;
    phone: string;
    address: AddressDto;
}

/** Address as returned by API (includes id and coordinates) */
export interface AddressDto
{
    id: string;
    street: string;
    houseNumber: string;
    apartmentNumber: string | null;
    city: string;
    postalCode: string;
    country: string;
    latitude: number;
    longitude: number;
}

// Used in shipment list/table views (matches ShipmentListItemDto)
export interface ShipmentListItem
{
    id: string;
    trackingNumber: string;
    sender: string;
    receiver: string;
    destination: string;
    status: string;
    updatedAt: string;
}

// Full shipment details for the details page
export interface ShipmentDetails
{
    id: string;
    trackingNumber: string;

    senderId: string;
    receiverId: string;
    destinationAddressId: string;

    sender: ShipmentPartyDto;
    receiver: ShipmentPartyDto;
    destinationAddress: AddressDto;

    weight: number;
    serviceType: ServiceType;
    status: string;

    createdAt: string;
    updatedAt: string;
    estimatedDelivery: string | null;

    events: ShipmentEvent[];
}

/** Contact row from GET /api/contacts */
export interface ContactListItemDto
{
    id: string;
    name: string;
    email: string | null;
    phone: string | null;
}

/** Contact from GET /api/contacts/:id */
export interface ContactDetailsDto
{
    id: string;
    name: string;
    email: string;
    phone: string;
    primaryAddress: AddressDto;
    destinationAddresses: AddressDto[];
}

// Generic paginated API response
export interface PagedResult<T>
{
    items: T[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}

// Query params for shipment list API
export interface ShipmentsQueryParams
{
    page?: number;
    pageSize?: number;
    trackingNumber?: string;
    status?: string;
    sortBy?: string;
    direction?: 'asc' | 'desc';
}

// Used for dropdowns (statuses, service types, etc.)
export interface MetadataOptionDto
{
    value: string;
    label: string;
}

export interface ShipmentsToolbarController
{
    statuses: MetadataOptionDto[];
    status: string;
    search: string;
    setStatus: (value: string) => void;
    setSearch: (value: string) => void;
    clear: () => void;
    refresh: () => void;
}

export interface ShipmentsController
{
    data: PagedResult<ShipmentListItem> | undefined;
    statuses: MetadataOptionDto[];
    status: string;
    search: string;
    sortModel: GridSortModel;
    page: number;
    pageSize: number;
    isLoading: boolean;
    isError: boolean;
    setStatus: (value: string) => void;
    setSearch: (value: string) => void;
    setSortModel: (model: GridSortModel) => void;
    setPage: (page: number) => void;
    setPageSize: (size: number) => void;
    refetch: () => void;
    handleRowClick: (params: { id: string | number }) => void;
    toolbar: ShipmentsToolbarController;
}
