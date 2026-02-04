import { useState } from "react"
import { useDebounce } from "@/hooks/useDebounce"
import type { GridSortModel } from "@mui/x-data-grid"
import type { ShipmentListItem } from "./types"

export function useShipmentQueryParams() {
    const [page, setPage] = useState(1)
    const [pageSize, setPageSize] = useState(10)
    const [status, setStatus] = useState("")
    const [search, setSearch] = useState("")
    const debouncedSearch = useDebounce(search, 600)

    type ShipmentSortField = keyof ShipmentListItem

    const [sortModel, setSortModel] = useState<GridSortModel>([
        { field: "updatedAt" as ShipmentSortField, sort: "desc" }
    ])

    const sortBy = sortModel[0]?.field
    const direction =
        sortModel[0]?.sort === "asc" || sortModel[0]?.sort === "desc"
            ? sortModel[0].sort
            : undefined

    return {
        page,
        pageSize,
        status,
        search,
        debouncedSearch,
        sortModel,

        setPage,
        setPageSize,
        setStatus,
        setSearch,
        setSortModel,

        queryParams: {
            page,
            pageSize,
            status: status || undefined,
            trackingNumber: debouncedSearch || undefined,
            sortBy,
            direction
        }
    }
}
