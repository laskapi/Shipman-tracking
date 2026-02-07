import { Route, Routes } from "react-router";
import LoginPage from "../features/auth/LoginPage";
import ProtectedRoute from "../features/auth/ProtectedRoute";
import RegisterPage from "../features/auth/RegisterPage";
import DashboardPage from "../features/dashboard/DashboardPage";
import ShipmentDetailsPage from "../features/shipments/ShipmentDetailsPage";
import ShipmentsPage from "../features/shipments/ShipmentsPage";

export function AppRouter() {
    return (
        <Routes>
            {/* Public routes */}
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />

            {/* Protected routes */}
            <Route element={<ProtectedRoute />}>
                <Route path="/" element={<DashboardPage />} />
                <Route path="/shipments" element={<ShipmentsPage />} />
                <Route path="/shipments/:id" element={<ShipmentDetailsPage />} />
            </Route>
        </Routes>
    );
}
