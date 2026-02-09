import { HeaderActionsType, setHeader } from "@/app/headerSlice"
import { Box, Divider, Paper } from "@mui/material"
import { useEffect } from "react"
import { useDispatch } from "react-redux"
import { useParams } from "react-router-dom"
import EventActionsPanel from "./components/EventActionPanel"
import ShipmentMetadata from "./components/ShipmentMetadata"
import ShipmentSummary from "./components/ShipmentSummary"
import TimelinePanel from "./components/TimelinePanel"
import { useGetShipmentByIdQuery } from "./shipmentsApi"

export default function ShipmentDetailsPage() {
    const { id } = useParams()
    const dispatch = useDispatch()

    const { data: shipment } =
        useGetShipmentByIdQuery(id!, { skip: !id })

    useEffect(() => {
        if (shipment) {
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
    })


    if (!shipment) return null

    return (
        <Box>
            <Box
                sx={{
                    display: "grid",
                    gridTemplateColumns: {
                        xs: "1fr",
                        md: "minmax(500px, 1fr) 360px"
                    },
                    gap: 4
                }}
            >
                <Box sx={{ display: "flex", flexDirection: "column", gap: 3 }}>
                    <ShipmentSummary shipment={shipment} />
                    <ShipmentMetadata shipment={shipment} />
                </Box>

                <Box>
                    <Paper sx={{ p: 3, borderRadius: 3 }}>
                        <TimelinePanel events={shipment.events} />
                        <Divider sx={{ my: 3 }} />
                        <Box sx={{ mt: 2 }}>
                            <EventActionsPanel shipmentId={shipment.id} />
                        </Box>
                    </Paper>
                </Box>
            </Box>
        </Box>
    )
}
