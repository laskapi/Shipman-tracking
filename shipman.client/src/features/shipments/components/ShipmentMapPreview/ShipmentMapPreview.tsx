import { MapContainer, TileLayer, Marker } from "react-leaflet"
import { useMap } from "react-leaflet"
import L from "leaflet"
import "leaflet/dist/leaflet.css"
import { RouteLine } from "./RouteLine"
import type { ShipmentMapPreviewProps } from "./types"

function FitBounds({ origin, destination }: ShipmentMapPreviewProps) {
    const map = useMap()

    map.fitBounds(
        [
            [origin.lat, origin.lng],
            [destination.lat, destination.lng]
        ],
        { padding: [40, 40] }
    )

    return null
}

const originIcon = new L.Icon({
    iconUrl: "/icons/origin.svg",
    iconSize: [32, 32],
    iconAnchor: [16, 32],
})

const destinationIcon = new L.Icon({
    iconUrl: "/icons/destination.svg",
    iconSize: [32, 32],
    iconAnchor: [16, 32],
})


export default function ShipmentMapPreview({ origin, destination }: ShipmentMapPreviewProps) {
    return (
        <MapContainer
            center={[origin.lat, origin.lng]}
            zoom={5}
            style={{ height: "100%", width: "100%", borderRadius: 8 }}
            scrollWheelZoom={false}
        >
            <TileLayer
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                attribution="© OpenStreetMap contributors"
            />

            <FitBounds origin={origin} destination={destination} />

            <Marker position={[origin.lat, origin.lng]} icon={originIcon} />
            <Marker position={[destination.lat, destination.lng]} icon={destinationIcon} />
            <RouteLine origin={origin} destination={destination} />

        </MapContainer>
    )
}
