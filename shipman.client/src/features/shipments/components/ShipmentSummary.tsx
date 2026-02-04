import { Paper, Typography, Grid } from "@mui/material";
import StatusBadge from "@/components/StatusBadge";
import type { ShipmentDetails } from "@/features/shipments/types";

export default function ShipmentSummary({ shipment }: { shipment: ShipmentDetails }) {
    return (
        <Paper
            elevation={1}
            sx={{
                p: 3,
                borderRadius: 3,
                mb: 3,
            }}
        >
            <Typography variant="h6" mb={3} fontWeight={600}>
                Shipment Summary
            </Typography>

            <Grid container spacing={2}>
                <DetailItem label="Tracking Number" value={shipment.trackingNumber} />
                <DetailItem label="Status" value={<StatusBadge status={shipment.status} />} />
                <DetailItem label="Service Type" value={shipment.serviceType} />
                <DetailItem label="Weight" value={`${shipment.weight} kg`} />
                <DetailItem label="Sender" value={shipment.sender} />
                <DetailItem label="Receiver" value={shipment.receiver} />
                <DetailItem label="Origin" value={shipment.origin} />
                <DetailItem label="Destination" value={shipment.destination} />
                <DetailItem
                    label="Created At"
                    value={new Date(shipment.createdAt).toLocaleString()}
                />
                <DetailItem
                    label="Updated At"
                    value={new Date(shipment.updatedAt).toLocaleString()}
                />
                {shipment.estimatedDelivery && (
                    <DetailItem
                        label="Estimated Delivery"
                        value={new Date(shipment.estimatedDelivery).toLocaleString()}
                    />
                )}
            </Grid>
        </Paper>
    );
}

function DetailItem({ label, value }: { label: string; value: React.ReactNode }) {
    return (
        <Grid size={{ xs: 12, sm: 6 }}>
            <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
                {label}
            </Typography>
            <Typography variant="body1" fontWeight={500}>
                {value}
            </Typography>
        </Grid>
    );
}
