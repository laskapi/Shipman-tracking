import { useEffect } from "react"
import { useGetProfileQuery } from "./authApi"
import { useAppDispatch } from "@/app/storeHooks"
import { setAuthenticated, clearAuth } from "./authSlice"
export function AuthInitializer() {
    const { data, isSuccess, isError } = useGetProfileQuery()
    const dispatch = useAppDispatch()

    useEffect(() => {
        if (isSuccess && data) {
            dispatch(setAuthenticated())
        }
        if (isError) {
            dispatch(clearAuth())
        }
    }, [isSuccess, isError, data, dispatch])
    return null
}
