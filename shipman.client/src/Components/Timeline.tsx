import { Box, Typography } from "@mui/material";
import type { ShipmentEvent } from "@/types/shipment";

interface TimelineProps {
    events: ShipmentEvent[];
}

export default function Timeline({ events }: TimelineProps) {
    if (!events || events.length === 0) {
        return <Typography>No tracking events available.</Typography>;
    }

    const sorted = [...events].sort(
        (a, b) =>
            new Date(b.timestamp).getTime() -
            new Date(a.timestamp).getTime()
    );

    return (
        <Box sx={{ position: "relative", pl: 3 }}>
            {/* Vertical line */}
            <Box
                sx={{
                    position: "absolute",
                    left: 6,
                    top: 0,
                    bottom: 0,
                    width: 2,
                    bgcolor: "divider",
                }}
            />

            {sorted.map((event, index) => (
                <Box key={event.timestamp + index} sx={{ mb: 4, position: "relative" }}>
                    {/* Dot */}
                    <Box
                        sx={{
                            position: "absolute",
                            left: -1,
                            top: 4,
                            width: 12,
                            height: 12,
                            borderRadius: "50%",
                            bgcolor: "primary.main",
                            border: "2px solid white",
                            zIndex: 1,
                        }}
                    />

                    {/* Content */}
                    <Box sx={{ ml: 3 }}>
                        <Typography fontWeight={600}>
                            {new Date(event.timestamp).toLocaleString()}
                        </Typography>
                        <Typography color="text.secondary">
                            {event.description}
                        </Typography>
                    </Box>
                </Box>
            ))}
        </Box>
    );
}
