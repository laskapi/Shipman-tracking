import { Box, Typography } from "@mui/material"
import PanelHeader from "@/ui/PanelHeader"
import type { AddressDto, ShipmentDetails } from "@/features/shipments/types"

interface ShipmentRouteDetailsProps
{
    shipment: ShipmentDetails
}

function formatAddress(a: AddressDto): string
{
    const parts = [
        [a.street, a.houseNumber].filter(Boolean).join(" "),
        a.apartmentNumber ? `apt. ${a.apartmentNumber}` : null,
        [a.postalCode, a.city].filter(Boolean).join(" "),
        a.country
    ].filter(Boolean);
    return parts.join(", ");
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
                    rowGap: 2,
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
                        {formatAddress(shipment.sender.address)}
                    </Typography>
                </Box>

                <Box sx={{ overflowY: "auto", minHeight: 0 }}>
                    <Typography variant="body1" fontWeight={500}>
                        {formatAddress(shipment.destinationAddress)}
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
