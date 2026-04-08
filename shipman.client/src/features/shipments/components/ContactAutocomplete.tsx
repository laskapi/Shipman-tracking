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

import type { ContactListItemDto } from '../types';
import { useGetContactsQuery } from '../shipmentsApi';
import AddContactModal from './AddContactModal';

const ADD_NEW_ID = '__add_new__';

type Option = ContactListItemDto | { id: typeof ADD_NEW_ID; name: string; email: null; phone: null };

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

    const options: Option[] = [
        ...contacts,
        { id: ADD_NEW_ID, name: 'Add new contact', email: null, phone: null }
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
                            if (value?.id === ADD_NEW_ID)
                            {
                                setOpenModal(true);
                                return;
                            }
                            field.onChange(value?.id || "");
                        }}
                        options={options}
                        loading={isLoading}
                        getOptionLabel={option =>
                            option.id === ADD_NEW_ID
                                ? 'Add new contact'
                                : option.name
                        }
                        renderOption={(props, option) => (
                            <Box component="li" {...props}>
                                {option.id === ADD_NEW_ID ? (
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
                                inputProps={{
                                    ...params.inputProps,
                                    autoComplete: 'off',
                                }}
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
