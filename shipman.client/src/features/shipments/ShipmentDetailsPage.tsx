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
                display: "grid",
                gridTemplateColumns: {
                    xs: "1fr",
                    md: "3fr 1fr",
                },
                gap: 2,
                p: 2,
                alignItems: "stretch",
                overflowY: { xs: "auto", md: "hidden" }

            }}
        >
            {/* LEFT COLUMN */}

            <Paper
                sx={{
                    minWidth: 0,
                    minHeight: 0,
                    display: "flex",
                    flexDirection: {
                        xs: "column",
                        md: "row"
                    },
                    gap: 2,
                    p: 2,
                }}
            >
                {/* SUMMARY */}
                <Box
                    sx={{
                        flex: 1,
                    }}
                >
                    <ShipmentSummary shipment={shipment} />
                </Box>

                {/* TIMELINE */}
                <Box
                    sx={{
                        flex: "1 1 0",
                        display: "flex",
                        flexDirection: "column",
                        maxHeight: "100%",
                        p: 2
                    }}
                >
                    <Box sx={{
                        flex: "1 1 0",
                        overflowY: {
                            xs: "none",
                            md: "auto",
                        }
                    }
                    } >
                        <TimelinePanel events={shipment.events} />
                    </Box>

                    <Divider sx={{ my: 2 }} />
                    <EventActionsPanel shipmentId={shipment.id} />
                </Box>
            </Paper>

            {/* RIGHT COLUMN */}
            <Paper
                sx={{
                    p: 2,
                    display: "flex",
                    flexDirection: "column",
                    gap: 2,
                }}
            >
                <Box
                    sx={{
                        width: "100%",
                        maxWidth: "100%",
                        aspectRatio: "1 / 1",
                    }}
                >
                    <ShipmentMapPreview
                        origin={shipment.originCoordinates}
                        destination={shipment.destinationCoordinates}
                    />
                </Box>

                <ShipmentRouteDetails shipment={shipment} />
            </Paper>
        </Box>


    )
}
