import { Box, Card, CardContent, Container } from "@mui/material";
import { useDispatch } from "react-redux";
import CreateShipmentForm from "./CreateShipmentForm";
import { useEffect } from "react";
import { setHeader, HeaderActionsType } from "@/app/headerSlice";

export default function CreateShipmentPage()
{

    const dispatch = useDispatch()

    useEffect(() =>
    {
        dispatch(
            setHeader({
                title: "Create Shipment",
                subtitle: "",
                breadcrumb: [{ label: "Shipments", to: "/shipments" },
                {label:"Create Shipment"}],
                actionsType: HeaderActionsType.ShipmentsList
            })
        )
    }, [dispatch])
    return (
        <Box>
            <Container maxWidth="md">
                <Box display="flex" justifyContent="center" mt={4}>
                    <Card sx={{ width: "100%", p: 2 }}>
                        <CardContent>
                            <CreateShipmentForm />
                        </CardContent>
                    </Card>
                </Box>
            </Container>
        </Box>
    );
}
