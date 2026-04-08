import { FORM_STACK_SPACING } from "@/ui/formSpacing";
import {
    Alert,
    Box,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    Stack,
    TextField
} from '@mui/material';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';

import {
    createAddressSchema,
    type CreateAddressFormValues
} from '@/features/addresses/addressSchema';
import { firstApiValidationMessage } from '@/services/apiValidationMessage';
import { useAddContactDestinationAddressMutation } from '../shipmentsApi';

interface Props
{
    open: boolean;
    contactId: string | null;
    onClose: () => void;
    onCreated: (address: { id: string }) => void;
}

export default function AddAddressModal({
    open,
    contactId,
    onClose,
    onCreated
}: Props)
{
    const [addAddress, { isLoading }] = useAddContactDestinationAddressMutation();
    const [submitError, setSubmitError] = useState<string | null>(null);

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors }
    } = useForm<CreateAddressFormValues>({
        resolver: zodResolver(createAddressSchema),
        mode: 'onChange',
        defaultValues: {
            street: '',
            houseNumber: '',
            apartmentNumber: '',
            city: '',
            postalCode: '',
            country: ''
        }
    });

    useEffect(() =>
    {
        if (!open) setSubmitError(null);
    }, [open]);

    const onSubmit = async (values: CreateAddressFormValues) =>
    {
        if (!contactId) return;

        setSubmitError(null);
        try
        {
            const contact = await addAddress({
                contactId,
                address: values
            }).unwrap();

            const created = contact.destinationAddresses.at(-1);
            if (created) onCreated({ id: created.id });

            reset();
            onClose();
        }
        catch (e: unknown)
        {
            setSubmitError(
                firstApiValidationMessage(e) ?? 'Could not save address.'
            );
        }
    };

    const handleClose = () =>
    {
        setSubmitError(null);
        reset();
        onClose();
    };

    return (
        <Dialog
            open={open}
            onClose={handleClose}
            fullWidth
            maxWidth="sm"
            slotProps={{
                paper: {
                    sx: {
                        maxHeight: 'min(88dvh, 640px)',
                        display: 'flex',
                        flexDirection: 'column',
                    },
                },
            }}
        >
            <DialogTitle>Add destination address</DialogTitle>

            <DialogContent
                sx={{
                    flex: '1 1 auto',
                    minHeight: 0,
                    overflowY: 'auto',
                }}
            >
                <Stack spacing={FORM_STACK_SPACING} mt={0.5}>
                    <TextField
                        label="Street"
                        {...register('street')}
                        error={!!errors.street}
                        helperText={errors.street?.message}
                    />
                    <TextField
                        label="House number"
                        inputProps={{ inputMode: 'text' }}
                        {...register('houseNumber')}
                        error={!!errors.houseNumber}
                        helperText={errors.houseNumber?.message}
                    />
                    <TextField
                        label="Apartment (optional)"
                        {...register('apartmentNumber')}
                        error={!!errors.apartmentNumber}
                        helperText={errors.apartmentNumber?.message}
                    />
                    <TextField
                        label="City"
                        {...register('city')}
                        error={!!errors.city}
                        helperText={errors.city?.message}
                    />
                    <TextField
                        label="Postal code"
                        {...register('postalCode')}
                        error={!!errors.postalCode}
                        helperText={errors.postalCode?.message}
                    />
                    <TextField
                        label="Country"
                        {...register('country')}
                        error={!!errors.country}
                        helperText={errors.country?.message}
                    />
                </Stack>
            </DialogContent>

            {submitError ? (
                <Box sx={{ px: 3, pb: 1, flexShrink: 0 }}>
                    <Alert severity="warning" onClose={() => setSubmitError(null)}>
                        {submitError}
                    </Alert>
                </Box>
            ) : null}

            <DialogActions sx={{ flexShrink: 0, px: 3, pb: 2 }}>
                <Button onClick={handleClose}>Cancel</Button>
                <Button
                    onClick={handleSubmit(onSubmit)}
                    disabled={isLoading || !contactId}
                    variant="contained"
                >
                    Save
                </Button>
            </DialogActions>
        </Dialog>
    );
}
