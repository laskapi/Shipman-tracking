import
    {
        Autocomplete,
        TextField,
        CircularProgress,
        Box,
        Typography
    } from "@mui/material";
import { useState } from "react";
import { Controller } from "react-hook-form";

import { useGetAddressesQuery } from "../shipmentsApi";
import AddAddressModal from "./AddAddressModal";

interface Props
{
    name: string;
    label: string;
    control: any;
    setValue: any;
    error?: string;
}

export default function AddressAutocomplete({
    name,
    label,
    control,
    setValue,
    error
}: Props)
{
    const { data: addresses = [], isLoading } = useGetAddressesQuery();
    const [openModal, setOpenModal] = useState(false);

    const options = [
        ...addresses,
        { id: "__add_new__", street: "Add new address" }
    ];

    return (
        <>
            <Controller
                name={name}
                control={control}
                render={({ field }) => (
                    <Autocomplete
                        value={
                            addresses.find(a => a.id === field.value) || null
                        }
                        onChange={(_, value) =>
                        {
                            if (value?.id === "__add_new__")
                            {
                                setOpenModal(true);
                                return;
                            }
                            field.onChange(value?.id || null);
                        }}
                        options={options}
                        loading={isLoading}
                        getOptionLabel={option =>
                            option.id === "__add_new__"
                                ? "Add new address"
                                : option.street
                        }
                        renderOption={(props, option) => (
                            <Box component="li" {...props}>
                                {option.id === "__add_new__" ? (
                                    <Typography color="primary">
                                        ➕ Add new address
                                    </Typography>
                                ) : (
                                    <Box>
                                        <Typography fontWeight={500}>
                                            {option.street}
                                        </Typography>
                                        <Typography variant="body2" color="text.secondary">
                                            {option.city}, {option.country}
                                        </Typography>
                                    </Box>
                                )}
                            </Box>
                        )}
                        renderInput={params => (
                            <TextField
                                {...params}
                                label={label}
                                error={!!error}
                                helperText={error}
                                InputProps={{
                                    ...params.InputProps,
                                    endAdornment: (
                                        <>
                                            {isLoading ? (
                                                <CircularProgress size={20} />
                                            ) : null}
                                            {params.InputProps.endAdornment}
                                        </>
                                    )
                                }}
                            />
                        )}
                    />
                )}
            />

            <AddAddressModal
                open={openModal}
                onClose={() => setOpenModal(false)}
                onCreated={address =>
                {
                    setValue(name, address.id, { shouldValidate: true });
                    setOpenModal(false);
                }}
            />
        </>
    );
}
