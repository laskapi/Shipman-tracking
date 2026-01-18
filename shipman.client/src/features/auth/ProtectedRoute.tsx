import type { ReactNode } from "react"
import { useAppSelector } from "../../store/hooks"
import { Navigate } from "react-router-dom"

interface ProtectedRouteNodes {
    children: ReactNode
}
export default function ProtectedRoute({ children }: ProtectedRouteNodes) {
    const isAuthenticated = useAppSelector(state => state.auth.isAuthenticated)

    if (!isAuthenticated) return <Navigate to="/login" />

    return children
}



