import { Button, Box } from "@mui/material"
import { useSelector } from "react-redux"
import { useNavigate, useParams, Outlet } from "react-router"
import { HeaderActionsType } from "../app/headerSlice"
import { PageHeader } from "./PageHeader"
import type { RootState } from "../app/store"

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
        <Box
            sx={{
                display: "flex",
                flexDirection: "column",
                height: "100vh",

            }}
        >
            <PageHeader
                title={header.title}
                subtitle={header.subtitle}
                breadcrumb={header.breadcrumb}
                actions={actions}
            />

            <Box sx={{
                flexGrow: 1,
                overflow: "auto",
                px: { xs: 0, md: 3 },
                py: { xs: 0, md: 3 },
            }}>
                <Outlet />
            </Box>
        </Box>
    )
}
