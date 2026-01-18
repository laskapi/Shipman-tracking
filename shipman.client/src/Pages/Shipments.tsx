import { useState } from "react"
import { DataGrid } from "@mui/x-data-grid"
import type { GridColDef } from "@mui/x-data-grid"
import { Box, Typography } from "@mui/material"
import { useGetShipmentsQuery, useGetShipmentStatusesQuery } from "../services/shipmentApi"
import { ShipmentsToolbar } from "../Components/ShipmentsToolbar"
import { StatusBadge } from "../Components/StatusBadgeComponent"

export default function Shipments() {
    const { data: shipments, isLoading, isError, refetch } = useGetShipmentsQuery()
    const [statusFilter, setStatusFilter] = useState("")
    const [search, setSearch] = useState("")
    const handleClear = () => {
        setStatusFilter("")
        setSearch("")
    }
    const handleRefresh = () => {
        refetch()
    }

    const { data: statuses = [] } = useGetShipmentStatusesQuery()
    
    const filteredRows = (shipments ?? []).filter((s) =>
        statusFilter === "" ? true : s.status === statusFilter
    )
    const searchedRows = filteredRows.filter((f) =>
        f.trackingNumber.toLowerCase().includes(search.toLowerCase())
    )


    const columns: GridColDef[] = [
        { field: "id", headerName: "ID", width: 90, sortable: true },
        { field: "trackingNumber", headerName: "Tracking Number", flex: 1, sortable: true },
        {
            field: "status",
            headerName: "Status",
            width: 150,
            sortable: true,
            align: "center",
            renderCell: (params) =>
                <StatusBadge status={params.value }/>
                
            
        },
        {
            field: "createdAt",
            headerName: "Created At",
            width: 200,
            sortable: true,
            renderCell: (params) => {
                const value = params.value as string | null
                if (!value) return ""
                return new Date(value).toLocaleString("en-GB")
            },
            sortComparator: (v1, v2) =>
                new Date(v1 as string).getTime() - new Date(v2 as string).getTime()
        }
    ]

    if (isLoading) return <p>Loading shipments..</p>
    if (isError) return <p>Failed to load shipments</p>
    return (<>
        <Box sx={{ p: 4 }}>
            <Typography variant="h4" mb={3}>
                Shipments
            </Typography>

            <div style={{ height: 500, width: "100%" }}>
                <Box mb={2}>
                    <ShipmentsToolbar
                        statuses={statuses}
                        statusFilter={statusFilter}
                        setStatusFilter={setStatusFilter}
                        search={search}
                        setSearch={setSearch}
                        onClear={handleClear}
                        onRefresh={handleRefresh}
                    />
                </Box>

                <DataGrid
                    rows={searchedRows}
                    columns={columns}
                    loading={isLoading}
                    getRowId={(row) => row.id}
                    sortingMode="client"
                    initialState={{
                        sorting: {
                            sortModel: [{ field: "createdAt", sort: "desc" }]
                        }
                    }}
                />

            </div>
        </Box>
    </>
    )
}