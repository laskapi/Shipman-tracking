import { Box, Typography } from "@mui/material"
import PanelHeader from "@/ui/PanelHeader"
import type { ShipmentDetails } from "@/features/shipments/types"

interface ShipmentRouteDetailsProps
{
    shipment: ShipmentDetails
}

interface RouteItemProps
{
    label: string
    value: string
}

export function ShipmentRouteDetails({ shipment }: ShipmentRouteDetailsProps)
{
    return (
        <Box sx={{ flex: 1, overflow: "hidden" }}>
            <PanelHeader>Route</PanelHeader>

            <Box sx={{ mt: 1 }}>
                <RouteItem label="Origin" value={shipment.origin} />
                <RouteItem label="Destination" value={shipment.destination} />
                {shipment.estimatedDelivery && (
                    <RouteItem
                        label="ETA"
                        value={new Date(shipment.estimatedDelivery).toLocaleString()}
                    />
                )}
            </Box>
        </Box>
    )
}

function RouteItem({ label, value }: RouteItemProps)
{
    return (
        <Box sx={{ mb: 1 }}>
            <Typography variant="caption" color="text.secondary" sx={{ display: "block" }}>
                {label}
            </Typography>
            <Typography variant="body1" fontWeight={500}>
                {value}
            </Typography>
        </Box>
    )
}
