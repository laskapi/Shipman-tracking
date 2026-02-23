import { Box } from "@mui/material";
import Timeline from "./Timeline";
import PanelHeader from "@/ui/PanelHeader";
import type { ShipmentEvent } from "@/features/shipments/types";
export default function TimelinePanel({ events }: { events: ShipmentEvent[] })
{
    return (
        <Box>
            <Box
                sx={{
                    position: "sticky",
                    top: 0,
                    zIndex: 1,
                    backgroundColor: "background.paper",
                    pb: 1,
                }}
            >
                <PanelHeader>Tracking History</PanelHeader>
            </Box>

            <Timeline events={events} />

            <Box
                sx={{
                    position: "sticky",
                    bottom: 0,
                    height: "48px",
                    background: "linear-gradient(to bottom, rgba(255,255,255,0), rgba(255,255,255,1))",
                    pointerEvents: "none"
                }}
            />
        </Box>
    )
}

