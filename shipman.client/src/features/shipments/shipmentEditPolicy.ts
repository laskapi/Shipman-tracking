/** Matches server: ShipmentService blocks updates when Delivered or Cancelled. */
const LOCKED = new Set(['delivered', 'cancelled']);

export function isShipmentLockedForEdit(status: string | undefined): boolean
{
    if (!status) return false;
    return LOCKED.has(status.trim().toLowerCase());
}
