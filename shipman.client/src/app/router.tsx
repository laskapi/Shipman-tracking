import { Route, Routes } from "react-router"
import LoginPage from "../features/auth/LoginPage"
import ProtectedRoute from "../features/auth/ProtectedRoute"
import RegisterPage from "../features/auth/RegisterPage"
import DashboardPage from "../features/dashboard/DashboardPage"
import ShipmentDetailsPage from "../features/shipments/ShipmentDetailsPage"
import ShipmentsPage from "../features/shipments/ShipmentsPage"
import { PageLayout } from "@/ui/PageLayout"
import LogoutHandler from "./LogoutHandler"
import CreateShipmentPage from "../features/shipments/create/CreateShipmentPage"

export function AppRouter() {
    return (
        <Routes>
            {/* Public routes */}
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />

            {/* Protected routes */}
            <Route element={<ProtectedRoute />}>
                <Route element={<PageLayout />}>
                    <Route path="/" element={<DashboardPage />} />
                    <Route path="/shipments" element={<ShipmentsPage />} />
                    <Route path="/shipments/:id" element={<ShipmentDetailsPage />} />
                    <Route path="/shipments/create" element={<CreateShipmentPage />} />
                </Route>
                <Route path="/logout" element={<LogoutHandler />} />

            </Route>
        </Routes>
    )
}
