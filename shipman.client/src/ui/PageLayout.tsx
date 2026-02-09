import { useSelector } from "react-redux"
import { Outlet, useNavigate, useParams } from "react-router-dom"
import { Box, Button } from "@mui/material"
import { PageHeader } from "./PageHeader"
import { HeaderActionsType } from "../app/headerSlice"
import type { RootState } from "@/app/store"

export function PageLayout() {
    const header = useSelector((state: RootState) => state.header)
    const navigate = useNavigate()
    const { id } = useParams()

    let actions = null

    switch (header.actionsType) {
        case HeaderActionsType.ShipmentsList:
            actions = (
                <Button variant="contained" onClick={() => navigate("/shipments/new")}>
                    Add Shipment
                </Button>
            )
            break

        case HeaderActionsType.ShipmentDetails:
            actions = (
                <>
                    <Button variant="outlined" onClick={() => navigate(-1)}>
                        Back
                    </Button>
                    <Button
                        variant="contained"
                        onClick={() => navigate(`/shipments/${id}/edit`)}
                        sx={{ ml: 2 }}
                    >
                        Edit
                    </Button>
                </>
            )
            break

        case HeaderActionsType.Dashboard:
            actions = (
                <>
                    <Button variant="outlined" onClick={() => navigate("/logout")}>
                        Logout
                    </Button>
                    <Button
                        variant="contained"
                        onClick={() => navigate("/shipments")}
                        sx={{ ml: 2 }}
                    >
                        View Shipments
                    </Button>
                </>
            )
            break

    }

    return (
        <Box sx={{ p: 3 }}>
            <PageHeader
                title={header.title}
                subtitle={header.subtitle}
                breadcrumb={header.breadcrumb}
                actions={actions}
            />

            <Box sx={{ mt: 3 }}>
                <Outlet />
            </Box>
        </Box>
    )
}
