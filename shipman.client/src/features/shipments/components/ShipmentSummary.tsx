import { Box, Paper, Typography, Grid } from "@mui/material"
import type { ShipmentDetails } from "@/features/shipments/types"

interface Props {
    shipment: ShipmentDetails
}

export default function ShipmentSummary({ shipment }: Props) {
    return (
        <Paper
            elevation={1}
            sx={{
                p: 3,
                borderRadius: 3,
                mb: 3
            }}
        >
            <Typography variant="h6" sx={{ mb: 2 }}>
                Shipment Summary
            </Typography>

            <Grid container spacing={2}>
                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem label="Tracking Number" value={shipment.trackingNumber} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem label="Status" value={shipment.status} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem label="Sender" value={shipment.sender} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem
                        label="Receiver"
                        value={`${shipment.receiverName} (${shipment.receiverEmail}, ${shipment.receiverPhone})`}
                    />
                </Grid>

                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem label="Origin" value={shipment.origin} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem label="Destination" value={shipment.destination} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem label="Weight" value={`${shipment.weight} kg`} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem label="Service Type" value={shipment.serviceType} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem
                        label="Created At"
                        value={new Date(shipment.createdAt).toLocaleString()}
                    />
                </Grid>

                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem
                        label="Last Updated"
                        value={new Date(shipment.updatedAt).toLocaleString()}
                    />
                </Grid>

                <Grid size={{ xs: 12, sm: 6 }}>
                    <SummaryItem
                        label="Estimated Delivery"
                        value={
                            shipment.estimatedDelivery
                                ? new Date(shipment.estimatedDelivery).toLocaleString()
                                : "Not available"
                        }
                    />
                </Grid>
            </Grid>
        </Paper>
    )
}

function SummaryItem({ label, value }: { label: string; value: string }) {
    return (
        <Box>
            <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
                {label}
            </Typography>
            <Typography variant="body1" fontWeight={500}>
                {value}
            </Typography>
        </Box>
    )
}
