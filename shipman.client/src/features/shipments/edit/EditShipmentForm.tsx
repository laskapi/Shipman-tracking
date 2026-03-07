import
    {
        Box,
        Grid,
        Stack,
    } from "@mui/material";

import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import
    {
        editShipmentSchema,
        type EditShipmentDto
    } from "./editShipmentSchema";

import { PersonFields } from "../components/forms/PersonFields";
import { ServiceTypeField } from "../components/forms/ServiceTypeField";

import { FormCard } from "../components/forms/FormCard";
import { FormAction } from "../components/forms/FormAction";

interface Props
{
    initialValues: EditShipmentDto;
    onSubmit: (values: EditShipmentDto) => Promise<void>;
    isLoading: boolean;
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
        formState: { errors, isValid },
    } = useForm<EditShipmentDto>({
        resolver: zodResolver(editShipmentSchema),
        mode: "onChange",
        defaultValues: initialValues
    });

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <Box sx={{ display: "flex", flexDirection: "column", height: "100%" }}>
                <Grid container spacing={2}>

                    {/* Receiver */}
                    <Grid size={{ xs: 12, md: 6 }}>
                        <FormCard title="Receiver">
                            <PersonFields
                                register={register}
                                errors={errors}
                                prefix="receiver"
                            />
                        </FormCard>
                    </Grid>

                    {/* Shipment Details */}
                    <Grid size={{ xs: 12, md: 6 }}>
                        <Box sx={{ height: "100%", display: "flex", flexDirection: "column" }}>
                            <FormCard title="Shipment Details">
                                <Stack spacing={1}>
                                    <ServiceTypeField register={register} errors={errors} />
                                </Stack>
                            </FormCard>

                            <FormAction
                                isValid={isValid}
                                isLoading={isLoading}
                                label="Update Shipment"
                            />
                        </Box>
                    </Grid>

                </Grid>
            </Box>
        </form>
    );
}
