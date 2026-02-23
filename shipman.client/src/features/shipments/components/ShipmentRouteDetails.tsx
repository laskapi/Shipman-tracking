import { Box, Typography } from "@mui/material"
import PanelHeader from "@/ui/PanelHeader"
import type { ShipmentDetails } from "@/features/shipments/types"

interface ShipmentRouteDetailsProps
{
    shipment: ShipmentDetails
}

export function ShipmentRouteDetails({ shipment }: ShipmentRouteDetailsProps)
{
    const hasEta = Boolean(shipment.estimatedDelivery)

    return (
        <Box sx={{ flex: 1, overflow: "hidden" }}>
            <PanelHeader>Route</PanelHeader>

            <Box
                sx={{
                    width: "100%",
                    display: "grid",
                    gridTemplateColumns: hasEta ? "repeat(3, 1fr)" : "repeat(2, 1fr)",
                    rowGap: 0.5,
                    columnGap: 2,
                }}
            >
                {/* Labels row */}
                <Typography variant="caption" color="text.secondary" sx={{ alignSelf: "center" }}>
                    Origin
                </Typography>
                <Typography variant="caption" color="text.secondary" sx={{ alignSelf: "center" }}>
                    Destination
                </Typography>
                {hasEta && (
                    <Typography variant="caption" color="text.secondary" sx={{ alignSelf: "center" }}>
                        Estimated Delivery
                    </Typography>
                )}

                {/* Values row */}
                <Typography variant="body1" fontWeight={500}>
                    {shipment.origin}
                </Typography>
                <Typography variant="body1" fontWeight={500}>
                    {shipment.destination}
                </Typography>
                {hasEta && (
                    <Typography variant="body1" fontWeight={500}>
                        {new Date(shipment.estimatedDelivery!).toLocaleString()}
                    </Typography>
                )}
            </Box>
        </Box>
    )
}

