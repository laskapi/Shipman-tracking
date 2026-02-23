import type { ShipmentDetails } from "@/features/shipments/types"
import PanelHeader from "@/ui/PanelHeader"
import { Box, Grid, Typography } from "@mui/material"

interface Props
{
    shipment: ShipmentDetails
}

export default function ShipmentSummary({ shipment }: Props)
{
    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "column",
                minWidth: 0,
            }}
        >
            <PanelHeader>Shipment Summary</PanelHeader>

            <Grid container spacing={1.5}>
                <Grid size={{ xs: 12, md: 6 }}>
                    <SummaryItem label="Tracking Number" value={shipment.trackingNumber} />
                </Grid>

                <Grid size={{ xs: 12, md: 6 }}>
                    <SummaryItem label="Status" value={shipment.status} />
                </Grid>

                <Grid size={{ xs: 12, md: 6 }}>
                    <SummaryItem label="Service Type" value={shipment.serviceType} />
                </Grid>

                <Grid size={{ xs: 12, md: 6 }}>
                    <SummaryItem label="Weight" value={`${shipment.weight} kg`} />
                </Grid>

                {/* Created At — full width for clean alignment */}
                <Grid size={12}>
                    <SummaryItem
                        label="Created At"
                        value={new Date(shipment.createdAt).toLocaleString()}
                    />
                </Grid>

                <PanelHeader>Parties</PanelHeader>

                <Grid size={12}>
                    <SummaryItem
                        label="Sender"
                        value={
                            <Box sx={{ lineHeight: 1.5 }}>
                                {shipment.sender.name} <br />
                                {shipment.sender.email} <br />
                                {shipment.sender.phone}
                            </Box>
                        }
                    />
                </Grid>

                <Grid size={12}>
                    <SummaryItem
                        label="Receiver"
                        value={
                            <Box sx={{ lineHeight: 1.5 }}>
                                {shipment.receiver.name} <br />
                                {shipment.receiver.email} <br />
                                {shipment.receiver.phone}
                            </Box>
                        }
                    />
                </Grid>
            </Grid>
        </Box>
    )
}

function SummaryItem({ label, value }: { label: string; value: React.ReactNode })
{
    return (
        <Box sx={{ mb: 0.5 }}>
            <Typography
                variant="caption"
                color="text.secondary"
                sx={{ display: "block" }}
            >
                {label}
            </Typography>

            <Typography variant="body1" fontWeight={500}>
                {value}
            </Typography>
        </Box>
    )
}
