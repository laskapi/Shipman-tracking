import { Box, Stack } from "@mui/material";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import
    {
        createShipmentSchema,
        type CreateShipmentDto
    } from "./createShipmentSchema";

import ContactAutocomplete from "../components/ContactAutocomplete";
import AddressAutocomplete from "../components/AddressAutocomplete";
import { WeightField } from "../components/forms/WeightField";
import { ServiceTypeField } from "../components/forms/ServiceTypeField";
import { FormAction } from "../components/forms/FormAction";

interface Props
{
    initialValues: CreateShipmentDto;
    onSubmit: (values: CreateShipmentDto) => Promise<void>;
    isLoading: boolean;
}

export default function CreateShipmentForm({
    initialValues,
    onSubmit,
    isLoading
}: Props)
{
    const {
        register,
        handleSubmit,
        control,
        setValue,
        formState: { errors, isValid }
    } = useForm<CreateShipmentDto>({
        resolver: zodResolver(createShipmentSchema),
        mode: "onChange",
        defaultValues: initialValues
    });

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <Stack spacing={3}>
                <ContactAutocomplete
                    name="senderId"
                    label="Sender"
                    control={control}
                    setValue={setValue}
                    error={errors.senderId?.message}
                />

                <ContactAutocomplete
                    name="receiverId"
                    label="Receiver"
                    control={control}
                    setValue={setValue}
                    error={errors.receiverId?.message}
                />

                <AddressAutocomplete
                    name="destinationAddressId"
                    label="Destination Address"
                    control={control}
                    setValue={setValue}
                    error={errors.destinationAddressId?.message}
                />

                <WeightField register={register} errors={errors} />
                <ServiceTypeField register={register} errors={errors} />

                <FormAction
                    isValid={isValid}
                    isLoading={isLoading}
                    label="Create Shipment"
                />
            </Stack>
        </form>
    );
}
