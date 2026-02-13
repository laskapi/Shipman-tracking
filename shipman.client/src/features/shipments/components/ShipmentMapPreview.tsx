import { MapContainer, TileLayer, Marker, Polyline } from "react-leaflet"
import L from "leaflet"
import "leaflet/dist/leaflet.css"

type LatLng = [number, number]

interface ShipmentMapPreviewProps {
    originCoordinates: { lat: number; lng: number }
    destinationCoordinates: { lat: number; lng: number }

}

const defaultIcon: L.Icon = new L.Icon({
    iconUrl: "https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png",
    iconSize: [25, 41],
    iconAnchor: [12, 41],
})

export function ShipmentMapPreview({ origin, destination }: ShipmentMapPreviewProps) {
    const center: LatLng = [
        (origin.lat + destination.lat) / 2,
        (origin.lng + destination.lng) / 2,
    ]

    return (
        <MapContainer
            center={center}
            zoom={5}
            style={{ height: 300, width: "100%", borderRadius: 8 }}
            scrollWheelZoom={false}
        >
            <TileLayer
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                attribution="© OpenStreetMap contributors"
            />

            <Marker position={[origin.lat, origin.lng] as LatLng} icon={defaultIcon} />
            <Marker position={[destination.lat, destination.lng] as LatLng} icon={defaultIcon} />

            <Polyline
                positions={[
                    [origin.lat, origin.lng] as LatLng,
                    [destination.lat, destination.lng] as LatLng,
                ]}
                pathOptions={{ color: "blue" }}
            />
        </MapContainer>
    )
}
