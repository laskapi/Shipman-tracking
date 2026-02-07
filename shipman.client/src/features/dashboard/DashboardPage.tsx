import { useGetProfileQuery, useLogoutMutation } from '@/features/auth/authApi'
import { Button, Typography, Box } from '@mui/material'
import { useNavigate } from 'react-router-dom'
import { useAppDispatch } from '@/app/storeHooks'
import { clearAuth } from '@/features/auth/authSlice'
import ShipmentsPage from '@/features/shipments/ShipmentsPage'
export default function DashboardPage() {
    const { data: user, isLoading } = useGetProfileQuery()
    const [logout] = useLogoutMutation()
    const navigate = useNavigate()
    const dispatch = useAppDispatch()
    const handleLogout = async () => {
        await logout().unwrap()
        dispatch(clearAuth()) 
        navigate("/login")
    }

    if (isLoading) return <p>Loading...</p>

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h4" mb={2}>
                Welcome, {user?.email}
            </Typography>
            <Button variant="outlined" onClick={handleLogout}>
                Logout
            </Button>
            
           {/* <Button
                variant="contained"
                sx={{ mt: 2 }}
                onClick={() => navigate("/shipments")}
            >
                View Shipments
            </Button>*/}
            <ShipmentsPage/>
        </Box>
    )
}
