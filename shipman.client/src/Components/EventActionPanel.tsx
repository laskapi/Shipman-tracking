import { useAddShipmentEventMutation, useGetShipmentEventTypesQuery } from "@/features/shipments/shipmentsApi";
import { Box, Button } from "@mui/material";
import PanelHeader from "./PanelHeader";

export default function EventActionsPanel({ shipmentId }: { shipmentId: string }) {
    const { data: eventTypes, isLoading: loadingTypes } = useGetShipmentEventTypesQuery();
    const [addEvent, { isLoading }] = useAddShipmentEventMutation();

    if (loadingTypes) return null;

    const handleClick = async (eventType: string) => {
        await addEvent({
            id: shipmentId,
            eventType          
        }).unwrap();
    };

    return (
        <Box>
            <PanelHeader>Update Shipment</PanelHeader>

            <Box sx={{ display: "flex", flexWrap: "wrap", gap: 1 }}>
                {eventTypes?.map(evt => (
                    <Button
                        key={evt.value}
                        size="small"
                        variant="outlined"
                        disabled={isLoading}
                        onClick={() => handleClick(evt.value)}
                        sx={{ textTransform: "none", fontWeight: 600 }}
                    >
                        {evt.label}
                    </Button>
                ))}
            </Box>
        </Box>

    );
}
