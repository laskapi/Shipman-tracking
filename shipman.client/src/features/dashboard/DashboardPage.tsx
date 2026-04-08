import { HeaderActionsType, setHeader } from "@/app/headerSlice"
import { useGetProfileQuery } from "@/features/auth/authApi"
import { Box, Typography } from "@mui/material"
import { useEffect } from "react"
import { useDispatch } from "react-redux"

export default function DashboardPage() {
    const { data: user, isLoading } = useGetProfileQuery()
    const dispatch = useDispatch()
             
    useEffect(() => {
        dispatch(
            setHeader({
                title: user ? `Welcome, ${user.email}` : "Dashboard",
                subtitle: "",
                breadcrumb: [{ label: "Dashboard", to: "/" }],
                actionsType: HeaderActionsType.Dashboard
            })
        )
    })
    
    if (isLoading) return <p>Loading...</p>

    return (
        <Box mt={2} mb={2}>
            <Typography variant="body1">
                This is your dashboard. More widgets coming soon.
            </Typography>
        </Box>
    )
}
