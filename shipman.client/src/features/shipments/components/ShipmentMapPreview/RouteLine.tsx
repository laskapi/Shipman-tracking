import { useEffect, useState } from "react"
import { Polyline } from "react-leaflet"
import type { ShipmentMapPreviewProps } from "./types"

type LatLng = [number, number]

export function RouteLine({ origin, destination }:ShipmentMapPreviewProps) {
    const [route, setRoute] = useState < LatLng[]>([])

    useEffect(() => {
        const fetchRoute = async () => {
            const url = `https://router.project-osrm.org/route/v1/driving/${origin.lng},${origin.lat};${destination.lng},${destination.lat}?overview=full&geometries=geojson`

            const res = await fetch(url)
            const data = await res.json()

            if (data.routes?.[0]?.geometry?.coordinates) {
                const coords = data.routes[0].geometry.coordinates.map(([lng, lat]:LatLng) => [lat, lng])
                setRoute(coords)
            }
        }

        fetchRoute()
    }, [origin, destination])

    if (route.length === 0) return null

    return <Polyline positions={route} pathOptions={{ color: "blue", weight: 4 }} />
}
