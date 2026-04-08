import { Stack, TextField } from "@mui/material";
import { FORM_STACK_SPACING } from "@/ui/formSpacing";
import type { UseFormRegister, FieldErrors } from "react-hook-form";

interface Props
{
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    register: UseFormRegister<any>;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    errors: FieldErrors<any>;
    prefix: string; // "sender" or "receiver"
}

export function PersonFields({ register, errors, prefix }: Props)
{
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const e = (errors?.[prefix] ?? {}) as Record<string, any>;

    return (
        <Stack spacing={FORM_STACK_SPACING}>
            <TextField
                fullWidth
                label="Name"
                {...register(`${prefix}.name`)}
                error={!!e.name}
                helperText={e.name?.message ?? " "}
                slotProps={{ formHelperText: { sx: { minHeight: "1.5em" } } }}
            />

            <TextField
                fullWidth
                label="Email"
                {...register(`${prefix}.email`)}
                error={!!e.email}
                helperText={e.email?.message ?? " "}
                slotProps={{ formHelperText: { sx: { minHeight: "1.5em" } } }}
            />

            <TextField
                fullWidth
                label="Phone"
                {...register(`${prefix}.phone`)}
                error={!!e.phone}
                helperText={e.phone?.message ?? " "}
                slotProps={{ formHelperText: { sx: { minHeight: "1.5em" } } }}
            />

            <TextField
                fullWidth
                label="Address"
                {...register(`${prefix}.address`)}
                error={!!e.address}
                helperText={e.address?.message ?? " "}
                slotProps={{ formHelperText: { sx: { minHeight: "1.5em" } } }}
            />
        </Stack>
    );
}
