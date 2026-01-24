import { useGetShipmentByIdQuery } from "@/services/shipmentsApi";
import { Box, Typography } from "@mui/material";
import { useParams } from "react-router-dom";
import ShipmentMetadata from "../components/ShipmentMetada";
import ShipmentSummary from "../components/ShipmentSummary";
import TimelinePanel from "../components/TimelinePanel";

export default function ShipmentDetailsPage() {
    const { id } = useParams();
    const { data: shipment, isLoading, isError } = useGetShipmentByIdQuery(id!);

    if (isLoading) return <p>Loading shipment...</p>;
    if (isError || !shipment) return <p>Shipment not found</p>;

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h4" mb={3}>
                Shipment Details
            </Typography>

            <Box
                sx={{
                    display: "grid",
                    gridTemplateColumns: "minmax(700px, 1fr) 360px",
                    gap: 4,
                    alignItems: "flex-start",
                }}
            >
                {/* LEFT COLUMN */}
                <Box>
                    <ShipmentSummary shipment={shipment} />
                    <ShipmentMetadata shipment={shipment} />
                </Box>

                {/* RIGHT COLUMN */}
                <Box>
                    <TimelinePanel events={shipment.events} />
                </Box>
            </Box>


        </Box>
    );
}
