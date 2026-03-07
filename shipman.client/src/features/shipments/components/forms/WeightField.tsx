import { TextField } from "@mui/material";
import type { UseFormRegister, FieldErrors } from "react-hook-form";

interface Props
{
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    register: UseFormRegister<any>;

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    errors: FieldErrors<any>;
}

export function WeightField({ register, errors }: Props)
{
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const e = errors?.weight as Record<string,any>| undefined;

    return (
        <TextField
            fullWidth
            type="number"
            label="Weight (kg)"
            {...register("weight", { valueAsNumber: true })}
            error={!!e}
            helperText={e?.message ?? " "}
            slotProps={{ formHelperText: { sx: { minHeight: "1.5em" } } }}
        />
    );
}
