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
    TextField,
    Typography
} from '@mui/material';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';

import {
    createContactSchema,
    type CreateContactFormValues
} from '@/features/contacts/contactSchema';
import { firstApiValidationMessage } from '@/services/apiValidationMessage';
import { useCreateContactMutation } from '../shipmentsApi';

interface Props
{
    open: boolean;
    onClose: () => void;
    onCreated: (contact: { id: string }) => void;
}

export default function AddContactModal({ open, onClose, onCreated }: Props)
{
    const [createContact, { isLoading }] = useCreateContactMutation();
    const [submitError, setSubmitError] = useState<string | null>(null);

    const {
        register,
        handleSubmit,
        reset,
        formState: { errors }
    } = useForm<CreateContactFormValues>({
        resolver: zodResolver(createContactSchema),
        mode: 'onChange',
        defaultValues: {
            name: '',
            email: '',
            phone: '',
            primaryAddress: {
                street: '',
                houseNumber: '',
                apartmentNumber: '',
                city: '',
                postalCode: '',
                country: ''
            }
        }
    });

    useEffect(() =>
    {
        if (!open) setSubmitError(null);
    }, [open]);

    const onSubmit = async (values: CreateContactFormValues) =>
    {
        setSubmitError(null);
        try
        {
            const contact = await createContact(values).unwrap();
            onCreated({ id: contact.id });
            reset();
            onClose();
        }
        catch (e: unknown)
        {
            setSubmitError(
                firstApiValidationMessage(e) ?? 'Could not save contact.'
            );
        }
    };

    const handleClose = () =>
    {
        setSubmitError(null);
        reset();
        onClose();
    };

    const pa = errors.primaryAddress;

    return (
        <Dialog
            open={open}
            onClose={handleClose}
            fullWidth
            maxWidth="sm"
            slotProps={{
                paper: {
                    sx: {
                        maxHeight: 'min(92dvh, 720px)',
                        display: 'flex',
                        flexDirection: 'column',
                    },
                },
            }}
        >
            <DialogTitle>Add contact</DialogTitle>

            <DialogContent
                sx={{
                    flex: '1 1 auto',
                    minHeight: 0,
                    overflowY: 'auto',
                }}
            >
                <Stack spacing={FORM_STACK_SPACING} mt={0.5}>
                    <TextField
                        label="Name"
                        {...register('name')}
                        error={!!errors.name}
                        helperText={errors.name?.message}
                    />
                    <TextField
                        label="Email"
                        {...register('email')}
                        error={!!errors.email}
                        helperText={errors.email?.message}
                    />
                    <TextField
                        label="Phone"
                        {...register('phone')}
                        error={!!errors.phone}
                        helperText={errors.phone?.message}
                    />

                    <Typography variant="subtitle2" color="text.secondary">
                        Primary address
                    </Typography>
                    <TextField
                        label="Street"
                        {...register('primaryAddress.street')}
                        error={!!pa?.street}
                        helperText={pa?.street?.message}
                    />
                    <TextField
                        label="House number"
                        inputProps={{ inputMode: 'text' }}
                        {...register('primaryAddress.houseNumber')}
                        error={!!pa?.houseNumber}
                        helperText={pa?.houseNumber?.message}
                    />
                    <TextField
                        label="Apartment (optional)"
                        {...register('primaryAddress.apartmentNumber')}
                        error={!!pa?.apartmentNumber}
                        helperText={pa?.apartmentNumber?.message}
                    />
                    <TextField
                        label="City"
                        {...register('primaryAddress.city')}
                        error={!!pa?.city}
                        helperText={pa?.city?.message}
                    />
                    <TextField
                        label="Postal code"
                        {...register('primaryAddress.postalCode')}
                        error={!!pa?.postalCode}
                        helperText={pa?.postalCode?.message}
                    />
                    <TextField
                        label="Country"
                        {...register('primaryAddress.country')}
                        error={!!pa?.country}
                        helperText={pa?.country?.message}
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
                    disabled={isLoading}
                    variant="contained"
                >
                    Save
                </Button>
            </DialogActions>
        </Dialog>
    );
}
