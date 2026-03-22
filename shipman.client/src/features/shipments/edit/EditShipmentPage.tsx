import { Box, Card, CardContent, Container } from "@mui/material";
import { useDispatch } from "react-redux";
import { useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";

import { setHeader, HeaderActionsType } from "@/app/headerSlice";
import
    {
        shipmentsApi,
        useGetShipmentByIdQuery,
        useUpdateShipmentMutation
    } from "../shipmentsApi";

import EditShipmentForm from "./EditShipmentForm";
import type { EditShipmentDto } from "../edit/editShipmentSchema";
import type { AppDispatch } from "@/app/store";

export default function EditShipmentPage()
{
    const navigate = useNavigate();
    const { id } = useParams();

    const { data: shipment, isLoading } = useGetShipmentByIdQuery(id!);
    const dispatch = useDispatch<AppDispatch>();
    const [updateShipment, { isLoading: isUpdating }] = useUpdateShipmentMutation();

    useEffect(() =>
    {
        dispatch(
            setHeader({
                title: "Edit Shipment",
                breadcrumb: [
                    { label: "Shipments", to: "/shipments" },
                    { label: "Shipment Details", to: `/shipments/${id}` },
                    { label: "Edit Shipment" }
                ],
                actionsType: HeaderActionsType.EditShipment
            })
        );
    }, [dispatch, id]);

    if (isLoading || !shipment) return null;

    const initialValues: EditShipmentDto = {
        receiverId: shipment.receiverId,
        destinationAddressId: shipment.destinationAddressId,
        serviceType: shipment.serviceType
    };

    const handleUpdate = async (values: EditShipmentDto) =>
    {
        const updated = await updateShipment({
            id: id!,
            data: values
        }).unwrap();

        dispatch(
            shipmentsApi.util.updateQueryData(
                "getShipmentById",
                updated.id,
                draft => Object.assign(draft, updated)
            )
        );

        navigate(`/shipments/${updated.id}`);
    };

    return (
        <Container maxWidth="sm">
            <Box mt={4}>
                <Card>
                    <CardContent>
                        <EditShipmentForm
                            shipment={shipment}
                            initialValues={initialValues}
                            onSubmit={handleUpdate}
                            isLoading={isUpdating}
                        />
                    </CardContent>
                </Card>
            </Box>
        </Container>
    );
}
