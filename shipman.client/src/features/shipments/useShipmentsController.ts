import { useNavigate } from "react-router"
import {
    shipmentsApi,
    useGetShipmentsQuery,
    useGetShipmentStatusesQuery
} from "./shipmentsApi"
import { useShipmentQueryParams } from "./useShipmentQueryParams"
import type { ShipmentsController } from "./types"

export function useShipmentsController(): ShipmentsController {
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

    const navigate = useNavigate()
    const prefetchShipment = shipmentsApi.usePrefetch("getShipmentById")

    const handleRowClick = (params: {id: string | number}) => {
        const id = params.id.toString()
        prefetchShipment(id)
        navigate(`/shipments/${id}`)
    }

    // ⭐ Toolbar controller
    const toolbar = {
        statuses,
        status,
        search,

        setStatus: (value:string) => {
            setStatus(value)
            setPage(1)
        },

        setSearch: (value:string) => {
            setSearch(value)
            setPage(1)
        },

        clear: () => {
            setStatus("")
            setSearch("")
            setPage(1)
        },

        refresh: () => {
            refetch()
            setPage(1)
        }
    }

    return {
        data,
        statuses,
        status,
        search,
        sortModel,
        page,
        pageSize,
        isLoading,
        isError,
        setStatus,
        setSearch,
        setSortModel,
        setPage,
        setPageSize,
        refetch,
        handleRowClick,
        toolbar
    }
}
