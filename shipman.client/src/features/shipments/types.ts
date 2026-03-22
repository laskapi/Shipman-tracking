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
export type ServiceType = "Standard" | "Express" | "Freight";

// Used in shipment list/table views
export interface ShipmentListItem
{
    id: string;
    trackingNumber: string;
    senderId: string;
    receiverId: string;
    destinationAddressId: string | null;
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
    destinationAddressId: string | null;

    weight: number;
    serviceType: ServiceType;
    status: string;

    createdAt: string;
    updatedAt: string;
    estimatedDelivery: string | null;

    events: ShipmentEvent[];
}

// Contact used for sender/receiver selection
export interface ContactDto
{
    id: string;
    name: string;
    email: string;
    phone: string;
    addressId: string | null;
}

// Address used for shipment destination
export interface AddressDto
{
    id: string;
    street: string;
    city: string;
    postalCode: string;
    country: string;
}

// Coordinates returned by backend for map display
export interface CoordinatesDto
{
    lat: number;
    lng: number;
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
    direction?: "asc" | "desc";
}

// Used for dropdowns (statuses, service types, etc.)
export interface MetadataOptionDto
{
    value: string;
    label: string;
}
