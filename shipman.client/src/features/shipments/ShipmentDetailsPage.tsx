import { Box, Divider, Paper } from "@mui/material"
import { useEffect } from "react"
import { useDispatch } from "react-redux"
import { useParams } from "react-router"
import { HeaderActionsType, setHeader } from "../../app/headerSlice"
import EventActionsPanel from "./components/EventActionPanel"
import ShipmentMapPreview from "./components/ShipmentMapPreview/ShipmentMapPreview"
import ShipmentSummary from "./components/ShipmentSummary"
import TimelinePanel from "./components/TimelinePanel"
import { useGetShipmentByIdQuery } from "./shipmentsApi"
import { ShipmentRouteDetails } from "./components/ShipmentRouteDetails"

export default function ShipmentDetailsPage()
{
    const { id } = useParams()

    const { data: shipment } =
        useGetShipmentByIdQuery(id!, { skip: !id })

    const dispatch = useDispatch()
    useEffect(() =>
    {
        if (shipment)
        {
            dispatch(
                setHeader({
                    title: `Shipment ${shipment.trackingNumber}`,
                    subtitle: shipment.status,
                    breadcrumb: [
                        { label: "Shipments", to: "/shipments" },
                        { label: shipment.trackingNumber }
                    ],
                    actionsType: HeaderActionsType.ShipmentDetails
                })
            )
        }
    }, [shipment, dispatch])
    if (!shipment)
    {
        return <div>Loading shipment...</div>
    }

    return (
        <Box
            sx={{
                display: "flex",
                height: "100%",
                gap: 2,
                p: 2,
            }}
        >
            {/* LEFT COLUMN */}
            <Box
                sx={{
                    display: "flex",
                    flex: 3,
                    flexDirection: "column",
                    gap: 2,
                }}
            >
                {/* Summary */}
                <ShipmentSummary shipment={shipment} />

                {/* Route + Map */}
                <Paper
                    sx={{
                        p: 2,
                        display: "flex",
                        minHeight: 0,
                        flex:1
                    }}
                >
                    {/* Route info */}
                    <ShipmentRouteDetails shipment={shipment} />

                    {/* Small map */}
                    <Box sx={{ flex: 1, minHeight: 0, height: "100%" }}>
                            <ShipmentMapPreview
                                origin={shipment.originCoordinates}
                                destination={shipment.destinationCoordinates}
                            />
                    </Box>
                </Paper>
            </Box>

            {/* RIGHT COLUMN */}

            <Paper
                sx={{
                    p: 2,
                    flex: "1 1 0",
                    display: "flex",
                    flexDirection: "column",
                    minHeight: 0,
                    overflow: "hidden",

                }}
            >
                <Box sx={{
                    flexGrow: 1, minHeight: 0, overflowY: "auto"
                }}>
                    <TimelinePanel events={shipment.events} />
                </Box>
                <Divider sx={{ my: 2 }} />
                <EventActionsPanel shipmentId={shipment.id} />
            </Paper>

        </Box>
    )
}
