import { Box, Paper } from "@mui/material"
import { DataGrid } from "@mui/x-data-grid"
import { ShipmentsToolbar } from "./components/ShipmentsToolbar"
import { shipmentsColumns } from "./shipmentColumns"
import type { ShipmentsController } from "./types"

export function DesktopShipmentsView({ ctrl }: { ctrl: ShipmentsController }) {
    const {
        data,
        sortModel,
        page,
        pageSize,
        setPage,
        setPageSize,
        setSortModel,
        handleRowClick
    } = ctrl

    return (
        <Paper
            elevation={1}
            sx={{
                p: 2,
                height: "100%",
                flexDirection: "column",
                overflow: "hidden",
                display: { xs: "none", md: "flex" }
            }}
        >
            <Box
                sx={{
                    position: "sticky",
                    top: 0,
                    zIndex: 10,
                    backgroundColor: "background.paper"
                }}
            >
                <ShipmentsToolbar ctrl={ctrl.toolbar} />
            </Box>

            <Box sx={{ flexGrow: 1, minHeight: 0 }}>
                <DataGrid
                    rows={data?.items ?? []}
                    rowCount={data?.totalCount ?? 0}
                    sortingMode="server"
                    paginationMode="server"
                    paginationModel={{
                        page: page - 1,
                        pageSize
                    }}
                    onPaginationModelChange={(m) => {
                        setPage(m.page + 1)
                        setPageSize(m.pageSize)
                    }}
                    sortModel={sortModel}
                    onSortModelChange={setSortModel}
                    pageSizeOptions={[5, 10, 20, 50]}
                    columns={shipmentsColumns}
                    getRowId={(row) => row.id}
                    onRowClick={handleRowClick}
                    disableRowSelectionOnClick
                />
            </Box>
        </Paper>
    )
}
