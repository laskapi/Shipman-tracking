import { TextField, MenuItem } from "@mui/material";
import type { UseFormRegister, FieldErrors } from "react-hook-form";

interface Props
{
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    register: UseFormRegister<any>;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    errors: FieldErrors<any>;
}

export function ServiceTypeField({ register, errors }: Props)
{
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const e = errors?.serviceType as Record<string, any> | undefined;

    return (
        <TextField
            select
            fullWidth
            label="Service Type"
            {...register("serviceType")}
            error={!!e}
            helperText={e?.message ?? " "}
            slotProps={{ formHelperText: { sx: { minHeight: "1.5em" } } }}
        >
            <MenuItem value="Standard">Standard</MenuItem>
            <MenuItem value="Express">Express</MenuItem>
            <MenuItem value="Freight">Freight</MenuItem>
        </TextField>
    );
}
