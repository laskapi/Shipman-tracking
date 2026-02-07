import { useAppSelector } from "@/app/storeHooks"
import { Navigate, Outlet } from "react-router-dom"

export default function ProtectedRoute() {
    const isAuthenticated = useAppSelector(state => state.auth.isAuthenticated)

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    return <Outlet />;
}

