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

import { useGetContactsQuery } from "../shipmentsApi";
import AddContactModal from "./AddContactModal";

interface Props
{
    name: string;
    label: string;
    control: any;
    setValue: any;
    error?: string;
}

export default function ContactAutocomplete({
    name,
    label,
    control,
    setValue,
    error
}: Props)
{
    const { data: contacts = [], isLoading } = useGetContactsQuery();
    const [openModal, setOpenModal] = useState(false);

    const options = [
        ...contacts,
        { id: "__add_new__", name: "Add new contact", email: "" }
    ];

    return (
        <>
            <Controller
                name={name}
                control={control}
                render={({ field }) => (
                    <Autocomplete
                        value={
                            contacts.find(c => c.id === field.value) || null
                        }
                        onChange={(_, value) =>
                        {
                            if (value?.id === "__add_new__")
                            {
                                setOpenModal(true);
                                return;
                            }
                            field.onChange(value?.id || "");
                        }}
                        options={options}
                        loading={isLoading}
                        getOptionLabel={option =>
                            option.id === "__add_new__"
                                ? "Add new contact"
                                : option.name
                        }
                        renderOption={(props, option) => (
                            <Box component="li" {...props}>
                                {option.id === "__add_new__" ? (
                                    <Typography color="primary">
                                        ➕ Add new contact
                                    </Typography>
                                ) : (
                                    <Box>
                                        <Typography fontWeight={500}>
                                            {option.name}
                                        </Typography>
                                        <Typography variant="body2" color="text.secondary">
                                            {option.email}
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

            <AddContactModal
                open={openModal}
                onClose={() => setOpenModal(false)}
                onCreated={contact =>
                {
                    setValue(name, contact.id, { shouldValidate: true });
                    setOpenModal(false);
                }}
            />
        </>
    );
}
