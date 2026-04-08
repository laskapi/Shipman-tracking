import {
    Autocomplete,
    TextField,
    CircularProgress,
    Box,
    Typography
} from '@mui/material';
import { useEffect, useMemo, useRef, useState } from 'react';
import { Controller, useWatch, type Control, type UseFormSetValue } from 'react-hook-form';

import type { AddressDto } from '../types';
import { useGetContactByIdQuery } from '../shipmentsApi';
import AddAddressModal from './AddAddressModal';

const ADD_NEW_ID = '__add_new__';

function formatAddressLine(a: AddressDto): string
{
    const line1 = [a.street, a.houseNumber].filter(Boolean).join(' ');
    const apt = a.apartmentNumber ? `, apt. ${a.apartmentNumber}` : '';
    return `${line1}${apt} · ${a.postalCode} ${a.city}, ${a.country}`;
}

type Row =
    | { id: typeof ADD_NEW_ID; label: string; sub: string; address: null }
    | { id: string; label: string; sub: string; address: AddressDto | null };

function buildRows(primary: AddressDto, destinations: AddressDto[]): Row[]
{
    const rows: Row[] = [
        {
            id: primary.id,
            label: formatAddressLine(primary),
            sub: 'Receiver primary',
            address: primary
        }
    ];

    const seen = new Set<string>([primary.id]);

    for (const d of destinations)
    {
        if (seen.has(d.id)) continue;
        seen.add(d.id);
        rows.push({
            id: d.id,
            label: formatAddressLine(d),
            sub: 'Additional destination',
            address: d
        });
    }

    rows.push({
        id: ADD_NEW_ID,
        label: 'Add new address',
        sub: 'For this receiver',
        address: null
    });

    return rows;
}

interface Props
{
    name: string;
    label: string;
    control: Control<any>;
    setValue: UseFormSetValue<any>;
    receiverFieldName: string;
    error?: string;
}

export default function AddressAutocomplete({
    name,
    label,
    control,
    setValue,
    receiverFieldName,
    error
}: Props)
{
    const receiverId = useWatch({ control, name: receiverFieldName }) as string | '';
    const destinationId = useWatch({ control, name }) as string | null | undefined;
    const hasReceiver = Boolean(receiverId && receiverId.length > 0);

    const prevReceiverRef = useRef<string | undefined>(undefined);

    useEffect(() =>
    {
        const prev = prevReceiverRef.current;
        if (prev !== undefined && prev !== receiverId && receiverId)
        {
            setValue(name, null, { shouldValidate: true });
        }
        prevReceiverRef.current = receiverId || undefined;
    }, [receiverId, name, setValue]);

    const { data: contactData, isLoading, isFetching } = useGetContactByIdQuery(
        receiverId,
        { skip: !hasReceiver }
    );

    const contact =
        contactData && contactData.id === receiverId ? contactData : undefined;

    const rows = useMemo(() =>
    {
        if (!contact) return [] as Row[];
        return buildRows(contact.primaryAddress, contact.destinationAddresses);
    }, [contact]);

    const selectedId =
        destinationId == null || destinationId === ''
            ? ''
            : String(destinationId);

    useEffect(() =>
    {
        if (!contact || !hasReceiver) return;

        const realIds = new Set(
            rows.filter(r => r.id !== ADD_NEW_ID).map(r => r.id)
        );
        const sid = selectedId;
        if (!sid || !realIds.has(sid))
        {
            setValue(name, contact.primaryAddress.id, { shouldValidate: true });
        }
    }, [contact, hasReceiver, rows, selectedId, name, setValue]);

    const options = useMemo(() =>
    {
        if (!hasReceiver) return [] as Row[];

        if (!contact && (isLoading || isFetching) && selectedId)
        {
            return [
                {
                    id: selectedId,
                    label: 'Loading address…',
                    sub: 'Fetching receiver addresses',
                    address: null
                } as Row,
                {
                    id: ADD_NEW_ID,
                    label: 'Add new address',
                    sub: 'For this receiver',
                    address: null
                }
            ];
        }

        if (!contact) return [] as Row[];

        if (selectedId && !rows.some(r => r.id === selectedId))
        {
            return [
                {
                    id: selectedId,
                    label: 'Current destination',
                    sub: 'Confirm or change below',
                    address: null
                } as Row,
                ...rows
            ];
        }

        return rows;
    }, [hasReceiver, contact, isLoading, isFetching, selectedId, rows]);

    const [openModal, setOpenModal] = useState(false);

    return (
        <>
            <Controller
                name={name}
                control={control}
                render={({ field }) =>
                {
                    const sid =
                        field.value == null || field.value === ''
                            ? ''
                            : String(field.value);
                    const value =
                        options.find(r => r.id === sid && r.id !== ADD_NEW_ID) ??
                        options.find(r => r.id === sid) ??
                        null;

                    return (
                        <Autocomplete<Row, false, false, false>
                            key={receiverId || 'no-receiver'}
                            disabled={!hasReceiver}
                            value={value}
                            onChange={(_, option) =>
                            {
                                if (!option) return;
                                if (option.id === ADD_NEW_ID)
                                {
                                    setOpenModal(true);
                                    return;
                                }
                                field.onChange(option.id);
                            }}
                            options={options}
                            loading={isLoading || isFetching}
                            getOptionLabel={option =>
                            {
                                if (!option) return '';
                                if (typeof option === 'string') return option;
                                return option.label ?? '';
                            }}
                            isOptionEqualToValue={(a, b) => a.id === b.id}
                            renderOption={(props, option) => (
                                <Box component="li" {...props}>
                                    {option.id === ADD_NEW_ID ? (
                                        <Typography color="primary">
                                            ➕ {option.label}
                                        </Typography>
                                    ) : (
                                        <Box>
                                            <Typography fontWeight={500}>
                                                {option.label}
                                            </Typography>
                                            <Typography
                                                variant="body2"
                                                color="text.secondary"
                                            >
                                                {option.sub}
                                            </Typography>
                                        </Box>
                                    )}
                                </Box>
                            )}
                            renderInput={params => (
                                <TextField
                                    {...params}
                                    label={label}
                                    placeholder={
                                        hasReceiver
                                            ? undefined
                                            : 'Select a receiver first'
                                    }
                                    error={!!error}
                                    helperText={
                                        error ??
                                        (!hasReceiver
                                            ? 'Choose the receiver first'
                                            : undefined)
                                    }
                                    inputProps={{
                                        ...params.inputProps,
                                        autoComplete: 'off',
                                    }}
                                    InputProps={{
                                        ...params.InputProps,
                                        endAdornment: (
                                            <>
                                                {isLoading || isFetching ? (
                                                    <CircularProgress size={20} />
                                                ) : null}
                                                {params.InputProps.endAdornment}
                                            </>
                                        )
                                    }}
                                />
                            )}
                        />
                    );
                }}
            />

            <AddAddressModal
                open={openModal}
                contactId={hasReceiver ? receiverId : null}
                onClose={() => setOpenModal(false)}
                onCreated={({ id }) =>
                {
                    setValue(name, id, { shouldValidate: true });
                    setOpenModal(false);
                }}
            />
        </>
    );
}
