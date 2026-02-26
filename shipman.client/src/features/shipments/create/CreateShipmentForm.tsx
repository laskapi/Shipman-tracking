import {
    Box,
    Button,
    Card,
    CardContent,
    CardHeader,
    Grid,
    MenuItem,
    Stack,
    TextField
} from "@mui/material";

import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";

import {
    createShipmentSchema,
    type CreateShipmentDto,
} from "./createShipmentSchema";

import { useCreateShipmentMutation } from "../shipmentsApi";

type BackendValidationError = {
    errors?: Record<string, string[]>;
};

function isFetchBaseQueryError(
    error: unknown
): error is { status: number; data: unknown }
{
    return (
        typeof error === "object" &&
        error !== null &&
        "status" in error &&
        "data" in error
    );
}

export default function CreateShipmentForm()
{
    const navigate = useNavigate();

    const [createShipment, { isLoading }] =
        useCreateShipmentMutation();

    const {
        register,
        handleSubmit,
        formState: { errors, isValid },
        setError,
    } = useForm<CreateShipmentDto>({
        resolver: zodResolver(createShipmentSchema),
        mode: "onChange"
    });

    const onSubmit = async (data: CreateShipmentDto) =>
    {
        try
        {
            const shipment = await createShipment(data).unwrap();
            navigate(`/shipments/${shipment.id}`);
        } catch (err: unknown)
        {
            if (isFetchBaseQueryError(err))
            {
                const backend = err.data as BackendValidationError;

                if (backend.errors)
                {
                    for (const key in backend.errors)
                    {
                        const messages = backend.errors[key];
                        const message = Array.isArray(messages)
                            ? messages[0]
                            : String(messages);

                        const formKey = key
                            .replace("Sender.", "sender.")
                            .replace("Receiver.", "receiver.")
                            .replace("Weight", "weight")
                            .replace("ServiceType", "serviceType");

                        setError(formKey as keyof CreateShipmentDto, {
                            type: "server",
                            message,
                        });
                    }
                }
            }
        }
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)}>

            <Box sx={{ display: "flex", flexDirection: "column", height: "100%" }}>
                <Grid
                    container
                    spacing={2}

                >

                    {/* Sender */}
                    <Grid size={{ xs: 12, md: 4 }}>
                        <Card >
                            <CardHeader title="Sender" />
                            <CardContent>
                                <Stack spacing={2}>
                                    <TextField
                                        fullWidth
                                        label="Name"
                                        {...register("sender.name")}
                                        error={!!errors.sender?.name}
                                        helperText={errors.sender?.name?.message}
                                    />

                                    <TextField
                                        fullWidth
                                        label="Email"
                                        {...register("sender.email")}
                                        error={!!errors.sender?.email}
                                        helperText={errors.sender?.email?.message}
                                    />

                                    <TextField
                                        fullWidth
                                        label="Phone"
                                        {...register("sender.phone")}
                                        error={!!errors.sender?.phone}
                                        helperText={errors.sender?.phone?.message}
                                    />

                                    <TextField
                                        fullWidth
                                        label="Address"
                                        {...register("sender.address")}
                                        error={!!errors.sender?.address}
                                        helperText={errors.sender?.address?.message}
                                    />
                                </Stack>
                            </CardContent>
                        </Card>
                    </Grid>

                    {/* Receiver */}
                    <Grid size={{ xs: 12, md: 4 }}>
                        <Card>
                            <CardHeader title="Receiver" />
                            <CardContent>
                                <Stack spacing={2}>
                                    <TextField
                                        fullWidth
                                        label="Name"
                                        {...register("receiver.name")}
                                        error={!!errors.receiver?.name}
                                        helperText={errors.receiver?.name?.message}
                                    />

                                    <TextField
                                        fullWidth
                                        label="Email"
                                        {...register("receiver.email")}
                                        error={!!errors.receiver?.email}
                                        helperText={errors.receiver?.email?.message}
                                    />

                                    <TextField
                                        fullWidth
                                        label="Phone"
                                        {...register("receiver.phone")}
                                        error={!!errors.receiver?.phone}
                                        helperText={errors.receiver?.phone?.message}
                                    />

                                    <TextField
                                        fullWidth
                                        label="Address"
                                        {...register("receiver.address")}
                                        error={!!errors.receiver?.address}
                                        helperText={errors.receiver?.address?.message}
                                    />
                                </Stack>
                            </CardContent>
                        </Card>
                    </Grid>

                    {/* Shipment Details */}
                    <Grid
                        size={{ xs: 12, md: 4 }}
                    >
                        <Box sx={{
                            height: "100%",
                            display: "flex",
                            flexDirection: "column",
                        }}>

                            <Card >
                                <CardHeader title="Shipment Details" />
                                <CardContent>
                                    <Stack spacing={2}>
                                        <TextField
                                            fullWidth
                                            type="number"
                                            label="Weight (kg)"
                                            {...register("weight", { valueAsNumber: true })}
                                            error={!!errors.weight}
                                            helperText={errors.weight?.message}
                                        />

                                        <TextField
                                            fullWidth
                                            select
                                            label="Service Type"
                                            defaultValue="Standard"
                                            {...register("serviceType")}
                                        >
                                            <MenuItem value="Standard">Standard</MenuItem>
                                            <MenuItem value="Express">Express</MenuItem>
                                            <MenuItem value="Priority">Priority</MenuItem>
                                        </TextField>
                                    </Stack>
                                </CardContent>
                            </Card>

                            <Box
                                sx={{
                                    display: "flex",
                                    flexDirection: "column",
                                    flexGrow: 1,
                                    justifyContent: "center",
                                    py: 2,
                                }}
                            >
                                <Button
                                    variant="contained"
                                    type="submit"
                                    disabled={!isValid || isLoading}
                                    size="large"
                                >
                                    {isLoading ? <>Creating...<br /></> : <>Create <br /> Shipment</>}
                                  
                                </Button>
                            </Box>
                        </Box>

                    </Grid>
                </Grid>
            </Box>
        </form>
    );
}
