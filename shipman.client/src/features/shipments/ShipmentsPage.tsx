import { useEffect } from "react"
import { useDispatch } from "react-redux"
import { setHeader, HeaderActionsType } from "../../app/headerSlice"
import { useShipmentsController } from "./useShipmentsController"
import { DesktopShipmentsView } from "./DesktopShipmentsView"
import { MobileShipmentsCards } from "./MobileShipmentsCards"

export default function ShipmentsPage() {
    const dispatch = useDispatch()

    useEffect(() => {
        dispatch(
            setHeader({
                title: "Shipments",
                subtitle: "",
                breadcrumb: [{ label: "Shipments", to: "/shipments" }],
                actionsType: HeaderActionsType.ShipmentsList
            })
        )
    }, [dispatch])

    const ctrl = useShipmentsController()

    if (ctrl.isLoading) return <p>Loading shipments...</p>
    if (ctrl.isError) return <p>Failed to load shipments</p>

    return (
        <>
            <DesktopShipmentsView ctrl={ctrl} />
            <MobileShipmentsCards ctrl={ctrl} />
        </>
    )
}
