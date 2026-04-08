import { useState } from 'react';
import {
    useAddShipmentEventMutation,
    useGetShipmentEventTypesQuery
} from '@/features/shipments/shipmentsApi';
import { Alert, Box, Button, Typography } from '@mui/material';
import PanelHeader from '@/ui/PanelHeader';
import { getAllowedShipmentEventValues } from '../shipmentEventFlow';

function messageFromApiError(e: unknown): string
{
    if (!e || typeof e !== 'object' || !('data' in e)) return 'Could not update status.';
    const data = (e as { data: unknown }).data;
    if (!data || typeof data !== 'object' || !('errors' in data))
        return 'Could not update status.';
    const errors = (data as { errors: Record<string, string[] | undefined> }).errors;
    const general = errors.General;
    if (Array.isArray(general) && general[0]) return general[0];
    const first = Object.values(errors).flat().find(Boolean);
    return typeof first === 'string' ? first : 'Could not update status.';
}

interface Props
{
    shipmentId: string;
    status: string;
}

export default function EventActionsPanel({ shipmentId, status }: Props)
{
    const { data: eventTypes, isLoading: loadingTypes } =
        useGetShipmentEventTypesQuery();
    const [addEvent, { isLoading }] = useAddShipmentEventMutation();
    const [error, setError] = useState<string | null>(null);

    const allowed = getAllowedShipmentEventValues(status);
    const actions = eventTypes?.filter(e => allowed.includes(e.value)) ?? [];

    const handleClick = async (eventType: string) =>
    {
        setError(null);
        try
        {
            await addEvent({
                id: shipmentId,
                eventType
            }).unwrap();
        }
        catch (e: unknown)
        {
            setError(messageFromApiError(e));
        }
    };

    if (loadingTypes) return null;

    return (
        <Box>
            <PanelHeader>Update shipment</PanelHeader>

            {error ? (
                <Alert severity="error" sx={{ mb: 2 }} onClose={() => setError(null)}>
                    {error}
                </Alert>
            ) : null}

            {actions.length === 0 ? (
                <Typography variant="body2" color="text.secondary">
                    No further status updates for this shipment.
                </Typography>
            ) : (
                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
                    {actions.map(evt => (
                        <Button
                            key={evt.value}
                            size="small"
                            variant="outlined"
                            disabled={isLoading}
                            onClick={() => handleClick(evt.value)}
                            sx={{ textTransform: 'none', fontWeight: 600 }}
                        >
                            {evt.label}
                        </Button>
                    ))}
                </Box>
            )}
        </Box>
    );
}
