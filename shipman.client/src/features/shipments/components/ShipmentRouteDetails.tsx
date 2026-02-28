import { Box, Typography } from "@mui/material"
import PanelHeader from "@/ui/PanelHeader"
import type { ShipmentDetails } from "@/features/shipments/types"

interface ShipmentRouteDetailsProps
{
    shipment: ShipmentDetails
}

export function ShipmentRouteDetails({ shipment }: ShipmentRouteDetailsProps)
{
    const hasEta = Boolean(shipment.estimatedDelivery);

    return (
        <Box sx={{ display: "flex", flexDirection: "column", height: "100%", overflow: "hidden" }}>
            <PanelHeader>Route</PanelHeader>

            <Box
                sx={{
                    flexShrink: 0,
                    display: "grid",
                    gridTemplateColumns: hasEta ? "repeat(3, 1fr)" : "repeat(2, 1fr)",
                    columnGap: 2,
                    rowGap: 0.5,
                }}
            >
                <Typography variant="caption" color="text.secondary">Origin</Typography>
                <Typography variant="caption" color="text.secondary">Destination</Typography>
                {hasEta && (
                    <Typography variant="caption" color="text.secondary">Estimated Delivery</Typography>
                )}
            </Box>

            <Box
                sx={{
                    flex: 1,
                    minHeight: 0,
                    display: "grid",
                    gridTemplateColumns: hasEta ? "repeat(3, 1fr)" : "repeat(2, 1fr)",
                    columnGap: 2,
                }}
            >
                <Box sx={{ overflowY: "auto", minHeight: 0 }}>
                    <Typography variant="body1" fontWeight={500}>
                        {shipment.origin}
                    </Typography>
                </Box>

                <Box sx={{ overflowY: "auto", minHeight: 0 }}>
                    <Typography variant="body1" fontWeight={500}>
                        {shipment.destination}
                    </Typography>
                </Box>

                {hasEta && (
                    <Box sx={{ overflowY: "auto", minHeight: 0 }}>
                        <Typography variant="body1" fontWeight={500}>
                            {new Date(shipment.estimatedDelivery!).toLocaleString()}
                        </Typography>
                    </Box>
                )}
            </Box>
        </Box>
    );
}
