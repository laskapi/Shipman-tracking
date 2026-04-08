/**
 * Allowed next event types from current shipment status (mirrors ShipmentEventFlow on the server).
 */
const ALLOWED_NEXT: Record<string, string[]> = {
    Created: ['Prepared', 'Cancelled'],
    Prepared: ['HandedOver', 'Cancelled', 'Delayed'],
    HandedOver: ['Delivered', 'Delayed'],
    Delivered: [],
    Delayed: ['Prepared', 'HandedOver', 'Delivered', 'Cancelled'],
    Cancelled: []
};

export function getAllowedShipmentEventValues(currentStatus: string): string[]
{
    return ALLOWED_NEXT[currentStatus] ?? [];
}
