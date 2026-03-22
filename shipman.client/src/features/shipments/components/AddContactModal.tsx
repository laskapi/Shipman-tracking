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
        createContactSchema,
        type CreateContactDto
    } from "../create/createContactSchema";
import { useCreateContactMutation } from "../shipmentsApi";

interface Props
{
    open: boolean;
    onClose: () => void;
    onCreated: (contact: any) => void;
}

export default function AddContactModal({ open, onClose, onCreated }: Props)
{
    const [createContact, { isLoading }] = useCreateContactMutation();

    const {
        register,
        handleSubmit,
        formState: { errors }
    } = useForm<CreateContactDto>({
        resolver: zodResolver(createContactSchema)
    });

    const onSubmit = async (values: CreateContactDto) =>
    {
        const contact = await createContact(values).unwrap();
        onCreated(contact);
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle>Add Contact</DialogTitle>

            <DialogContent>
                <Stack spacing={2} mt={1}>
                    <TextField
                        label="Name"
                        {...register("name")}
                        error={!!errors.name}
                        helperText={errors.name?.message}
                    />
                    <TextField
                        label="Email"
                        {...register("email")}
                        error={!!errors.email}
                        helperText={errors.email?.message}
                    />
                    <TextField
                        label="Phone"
                        {...register("phone")}
                        error={!!errors.phone}
                        helperText={errors.phone?.message}
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
