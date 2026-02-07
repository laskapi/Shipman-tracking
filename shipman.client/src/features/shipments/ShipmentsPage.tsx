import { ShipmentsToolbar } from "./components/ShipmentsToolbar"
import { useGetShipmentsQuery, useGetShipmentStatusesQuery } from "./shipmentsApi"
import { Box, Typography } from "@mui/material"
import { DataGrid, type GridRowParams } from "@mui/x-data-grid"
import { useNavigate } from "react-router-dom"
import { useShipmentQueryParams } from "./useShipmentQueryParams"
import { shipmentsColumns } from "./shipmentColumns"
import type { ShipmentListItem } from "./types"
export default function ShipmentsPage() {

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


    const { data, isLoading, isError, refetch } = useGetShipmentsQuery(queryParams)
    const { data: statuses = [] } = useGetShipmentStatusesQuery()
    const navigate = useNavigate()
    const handleRowClick = (params: GridRowParams<ShipmentListItem>) => {
        navigate(`/shipments/${params.id}`)
    }


    if (!data) return <p>Loading shipments..</p>
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
                </Box>
                <DataGrid
                    rows={data?.items ?? []}
                    rowCount={data?.totalCount ?? 0}
                    loading={isLoading}
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
            </div>
        </Box>
    </>
    )
}
