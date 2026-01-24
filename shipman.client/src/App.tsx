import { BrowserRouter, Routes, Route } from "react-router-dom"
import Login from "./pages/Login"
import Register from "./pages/Register"
import Dashboard from "./pages/DashboardPage"
import ProtectedRoute from "./features/auth/ProtectedRoute"
import { useGetProfileQuery } from "./services/authApi"
import { useAppDispatch } from "./store/hooks"
import { useEffect } from "react"
import { setAuthenticated, clearAuth } from "./store/authSlice"
import ShipmentsPage from "./pages/ShipmentsPage"
import ShipmentDetailsPage from "./pages/ShipmentDetailsPage"

export default function App() {
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

    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route
                    path="/"
                    element={
                        <ProtectedRoute>
                            <Dashboard />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/shipments"
                    element={
                        <ProtectedRoute>
                            <ShipmentsPage />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/shipments/:id"
                    element={
                        <ProtectedRoute>
                            <ShipmentDetailsPage />
                        </ProtectedRoute>
                    }
                />
                


            </Routes>
        </BrowserRouter>
    )
}
