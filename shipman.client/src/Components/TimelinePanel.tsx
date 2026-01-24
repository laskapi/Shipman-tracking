import { Paper, Typography } from "@mui/material";
import Timeline from "@/components/Timeline";
import type { ShipmentEvent } from "@/types/shipment";

export default function TimelinePanel({ events }: { events: ShipmentEvent[] }) {
    return (
        <Paper
            elevation={1}
            sx={{
                p: 3,
                borderRadius: 3,
            }}
        >
            <Typography variant="h6" mb={3} fontWeight={600}>
                Tracking History
            </Typography>

            <Timeline events={events} />
        </Paper>
    );
}
