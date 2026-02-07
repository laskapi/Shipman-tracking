import { useGetShipmentByIdQuery } from "./shipmentsApi";
import { Box, Divider, Paper, Typography } from "@mui/material";
import { useParams } from "react-router-dom";
import ShipmentMetadata from "./components/ShipmentMetadata";
import ShipmentSummary from "./components/ShipmentSummary";
import TimelinePanel from "@/components/TimelinePanel";
import EventActionsPanel from "../../components/EventActionPanel";

export default function ShipmentDetailsPage() {
    const { id } = useParams();
    const { data: shipment, isLoading, isError } = useGetShipmentByIdQuery(id!);

    if (isLoading) return <p>Loading shipment...</p>;
    if (isError || !shipment) return <p>Shipment not found</p>;

    return (
        <Box sx={{ p: { xs: 2, md: 4 } }}>
            <Typography
                variant="h4"
                mb={3}
                sx={{
                    position: "sticky",
                    top: 0,
                    backgroundColor: "white",
                    zIndex: 10,
                    pb: 2
                }}
            >
                Shipment Details
            </Typography>

            <Box
                sx={{
                    display: "grid",
                    gridTemplateColumns: {
                        xs: "1fr",
                        md: "minmax(500px, 1fr) 360px"
                    },
                    gap: 4
                }}
            >
                {/* LEFT COLUMN */}
                <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
                    <ShipmentSummary shipment={shipment} />
                    <ShipmentMetadata shipment={shipment} />
                </Box>

                {/* RIGHT COLUMN */}
                <Box>
                    <Paper sx={{ p: 3, borderRadius: 3 }}>
                        <TimelinePanel events={shipment.events} />
                        <Divider sx={{ my: 3 }} />
                        <Box sx={{ mt: 2 }}>
                            <EventActionsPanel shipmentId={shipment.id} />
                        </Box>
                    </Paper>
                </Box>

            </Box>
        </Box>


    );
}
