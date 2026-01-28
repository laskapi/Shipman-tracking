import { Routes, Route } from "react-router-dom"
import ProtectedRoute from "@/features/auth/ProtectedRoute"
import LoginPage from "@/features/auth/LoginPage"
import RegisterPage from "@/features/auth/RegisterPage"
import DashboardPage from "@/features/dashboard/DashboardPage"
import ShipmentsPage from "@/features/shipments/ShipmentsPage"
import ShipmentDetailsPage from "@/features/shipments/ShipmentDetailsPage"

export function AppRouter() {
    return (
        <Routes>
            {/* Public routes */}
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />

            {/* Protected routes */}
            <Route
                path="/"
                element={
                    <ProtectedRoute>
                        <DashboardPage />
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
    )
}
