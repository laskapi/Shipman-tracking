import { Box, Paper } from "@mui/material"
import { DataGrid, type GridRowParams } from "@mui/x-data-grid"
import { useEffect } from "react"
import { useDispatch } from "react-redux"
import { useNavigate } from "react-router-dom"
import { ShipmentsToolbar } from "./components/ShipmentsToolbar"
import { shipmentsColumns } from "./shipmentColumns"
import {
    shipmentsApi,
    useGetShipmentsQuery,
    useGetShipmentStatusesQuery
} from "./shipmentsApi"
import type { ShipmentListItem } from "./types"
import { useShipmentQueryParams } from "./useShipmentQueryParams"

import { HeaderActionsType, setHeader } from "@/app/headerSlice"

export default function ShipmentsPage() {
    const dispatch = useDispatch()
    const navigate = useNavigate()

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

    // Query params (pagination, sorting, filters)
    const {
        page,
        pageSize,
        status,
        search,
        sortModel,
        setPage,
        setPageSize,
        setStatus,
        setSearch,
        setSortModel,
        queryParams
    } = useShipmentQueryParams()

    const { data, isLoading, isError, refetch } =
        useGetShipmentsQuery(queryParams)

    const { data: statuses = [] } = useGetShipmentStatusesQuery()

    const prefetchShipment = shipmentsApi.usePrefetch("getShipmentById")

    const handleRowClick = (params: GridRowParams<ShipmentListItem>) => {
        const id = params.id.toString()
        prefetchShipment(id)
        navigate(`/shipments/${id}`)
    }

    if (isLoading) return <p>Loading shipments...</p>
    if (isError) return <p>Failed to load shipments</p>

    return (
        <Paper elevation={1} sx={{ p: 2 }}>
            <ShipmentsToolbar
                statuses={statuses}
                statusFilter={status}
                setStatusFilter={(value) => {
                    setStatus(value)
                    setPage(1)
                }}
                search={search}
                setSearch={(value) => {
                    setSearch(value)
                    setPage(1)
                }}
                onClear={() => {
                    setStatus("")
                    setSearch("")
                    setPage(1)
                }}
                onRefresh={() => {
                    refetch()
                    setPage(1)
                }}
            />

            <Box sx={{ height: 600, mt: 2 }}>
                <DataGrid
                    rows={data?.items ?? []}
                    rowCount={data?.totalCount ?? 0}
                    loading={false} // prevents skeleton flicker
                    sortingMode="server"
                    paginationMode="server"
                    paginationModel={{
                        page: page - 1,
                        pageSize: pageSize
                    }}
                    onPaginationModelChange={(model) => {
                        setPage(model.page + 1)
                        setPageSize(model.pageSize)
                    }}
                    sortModel={sortModel}
                    onSortModelChange={(model) => setSortModel(model)}
                    pageSizeOptions={[5, 10, 20, 50]}
                    columns={shipmentsColumns}
                    getRowId={(row) => row.id}
                    onRowClick={handleRowClick}
                />
            </Box>
        </Paper>
    )
}
