import
    {
        Dialog,
        DialogTitle,
        DialogContent,
        DialogActions,
        Button,
        Stack,
        TextField
    } from "@mui/material";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import
    {
        createAddressSchema,
        type CreateAddressDto
    } from "../create/createAddressSchema";
import { useCreateAddressMutation } from "../shipmentsApi";

interface Props
{
    open: boolean;
    onClose: () => void;
    onCreated: (address: any) => void;
}

export default function AddAddressModal({ open, onClose, onCreated }: Props)
{
    const [createAddress, { isLoading }] = useCreateAddressMutation();

    const {
        register,
        handleSubmit,
        formState: { errors }
    } = useForm<CreateAddressDto>({
        resolver: zodResolver(createAddressSchema)
    });

    const onSubmit = async (values: CreateAddressDto) =>
    {
        const address = await createAddress(values).unwrap();
        onCreated(address);
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle>Add Address</DialogTitle>

            <DialogContent>
                <Stack spacing={2} mt={1}>
                    <TextField
                        label="Street"
                        {...register("street")}
                        error={!!errors.street}
                        helperText={errors.street?.message}
                    />
                    <TextField
                        label="City"
                        {...register("city")}
                        error={!!errors.city}
                        helperText={errors.city?.message}
                    />
                    <TextField
                        label="Postal Code"
                        {...register("postalCode")}
                        error={!!errors.postalCode}
                        helperText={errors.postalCode?.message}
                    />
                    <TextField
                        label="Country"
                        {...register("country")}
                        error={!!errors.country}
                        helperText={errors.country?.message}
                    />
                </Stack>
            </DialogContent>

            <DialogActions>
                <Button onClick={onClose}>Cancel</Button>
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
