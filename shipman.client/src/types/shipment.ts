export interface ShipmentEvent {
    timestamp: string;
    description: string;
}

export interface ShipmentDetails {
    id: string;
    trackingNumber: string;
    sender: string;
    receiver: string;
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
