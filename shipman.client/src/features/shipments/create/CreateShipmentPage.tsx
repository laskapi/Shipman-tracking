import { Box, Card, CardContent, Container } from "@mui/material";
import { useDispatch } from "react-redux";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { setHeader, HeaderActionsType } from "@/app/headerSlice";
import { shipmentsApi, useCreateShipmentMutation } from "../shipmentsApi";

import CreateShipmentForm from "./CreateShipmentForm";
import type { CreateShipmentDto } from "../create/createShipmentSchema";
import type { AppDispatch } from "@/app/store";

export default function CreateShipmentPage()
{
    const navigate = useNavigate();
    const dispatch = useDispatch<AppDispatch>();

    const [createShipment, { isLoading }] = useCreateShipmentMutation();

    useEffect(() =>
    {
        dispatch(
            setHeader({
                title: "Create Shipment",
                breadcrumb: [
                    { label: "Shipments", to: "/shipments" },
                    { label: "Create Shipment" }
                ],
                actionsType: HeaderActionsType.CreateShipment
            })
        );
    }, [dispatch]);

    const handleCreate = async (values: CreateShipmentDto) =>
    {
        const shipment = await createShipment(values).unwrap();

        dispatch(
            shipmentsApi.util.updateQueryData(
                "getShipmentById",
                shipment.id,
                draft => Object.assign(draft, shipment)
            )
        );

        navigate(`/shipments/${shipment.id}`);
    };

    return (
        <Container maxWidth="sm">
            <Box mt={4}>
                <Card>
                    <CardContent>
                        <CreateShipmentForm
                            onSubmit={handleCreate}
                            isLoading={isLoading}
                            initialValues={{
                                senderId: "",
                                receiverId: "",
                                destinationAddressId: null,
                                weight: 0,
                                serviceType: "Standard"
                            }}
                        />
                    </CardContent>
                </Card>
            </Box>
        </Container>
    );
}
