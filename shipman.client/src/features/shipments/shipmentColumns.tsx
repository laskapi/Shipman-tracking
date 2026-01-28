import type { GridColDef, GridRenderCellParams } from "@mui/x-data-grid"
import type { ShipmentListItem } from "./types"
import StatusBadge from "@/components/StatusBadge"

export const shipmentsColumns: GridColDef<ShipmentListItem>[] = [
    {
        field: "trackingNumber",
        headerName: "Tracking Number",
        flex: 1,
        minWidth: 150,
    },
    {
        field: "origin",
        headerName: "Origin",
        flex: 1,
        minWidth: 120,
    },
    {
        field: "destination",
        headerName: "Destination",
        flex: 1,
        minWidth: 120,
    },
    {
        field: "status",
        headerName: "Status",
        flex: 1,
        minWidth: 120,
        renderCell: (params: GridRenderCellParams<ShipmentListItem>) => (
            <StatusBadge status={params.value} />
        ),
    },
    {
        field: "lastUpdated",
        headerName: "Last Updated",
        flex: 1,
        minWidth: 160,
        valueGetter: (value) => new Date(value).toLocaleString(),
    },
]
