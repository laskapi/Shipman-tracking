import { Box, Paper, Typography, Grid } from "@mui/material"
import type { ShipmentDetails } from "@/features/shipments/types"
import PanelHeader from "../../../ui/PanelHeader"

interface Props
{
    shipment: ShipmentDetails
}

export default function ShipmentSummary({ shipment }: Props)
{
    return (
        <Paper>
            <PanelHeader>Shipment Summary</PanelHeader>

            <Grid container spacing={1.5}>
                {/* Shipment Info */}
                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                    <SummaryItem label="Tracking Number" value={shipment.trackingNumber} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                    <SummaryItem label="Status" value={shipment.status} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                    <SummaryItem label="Service Type" value={shipment.serviceType} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                    <SummaryItem label="Weight" value={`${shipment.weight} kg`} />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                    <SummaryItem
                        label="Estimated Delivery"
                        value={
                            shipment.estimatedDelivery
                                ? new Date(shipment.estimatedDelivery).toLocaleString()
                                : "Not available"
                        }
                    />
                </Grid>

                {/* Parties */}
                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                    <SummaryItem
                        label="Sender"
                        value={`${shipment.sender.name} (${shipment.sender.email})`}
                    />
                </Grid>

                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                    <SummaryItem
                        label="Receiver"
                        value={`${shipment.receiver.name} (${shipment.receiver.email})`}
                    />
                </Grid>

                {/* Metadata */}
                <Grid size={{ xs: 12, sm: 6, md: 4 }}>
                    <SummaryItem
                        label="Created At"
                        value={new Date(shipment.createdAt).toLocaleString()}
                    />
                </Grid>
            </Grid>
        </Paper>
    )
}

function SummaryItem({ label, value }: { label: string; value: string })
{
    return (
        <Box sx={{ mb: 0.5 }}>
            <Typography variant="caption" color="text.secondary" sx={{ display: "block" }}>
                {label}
            </Typography>
            <Typography variant="body1" fontWeight={500} noWrap>
                {value}
            </Typography>
        </Box>
    )
}
