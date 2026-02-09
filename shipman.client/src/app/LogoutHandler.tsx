import { useEffect } from "react"
import { useNavigate } from "react-router"
import { useAppDispatch } from "./storeHooks"
import { useLogoutMutation } from "@/features/auth/authApi"
import { clearAuth } from "@/features/auth/authSlice"

export default function LogoutHandler() {
    const [logout] = useLogoutMutation()
    const dispatch = useAppDispatch()
    const navigate = useNavigate()

    useEffect(() => {
        logout().unwrap().finally(() => {
            dispatch(clearAuth())
            navigate("/login")
        })
    })

    return null
}
