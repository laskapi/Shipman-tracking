import { Stack } from "@mui/material";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import
    {
        editShipmentSchema,
        type EditShipmentDto
    } from "./editShipmentSchema";

import ContactAutocomplete from "../components/ContactAutocomplete";
import AddressAutocomplete from "../components/AddressAutocomplete";
import { ServiceTypeField } from "../components/forms/ServiceTypeField";
import { FormAction } from "../components/forms/FormAction";

interface Props
{
    initialValues: EditShipmentDto;
    onSubmit: (values: EditShipmentDto) => Promise<void>;
    isLoading: boolean;
    shipment: any;
}

export default function EditShipmentForm({
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
    } = useForm<EditShipmentDto>({
        resolver: zodResolver(editShipmentSchema),
        mode: "onChange",
        defaultValues: initialValues
    });

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <Stack spacing={3}>
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

                <ServiceTypeField register={register} errors={errors} />

                <FormAction
                    isValid={isValid}
                    isLoading={isLoading}
                    label="Update Shipment"
                />
            </Stack>
        </form>
    );
}
