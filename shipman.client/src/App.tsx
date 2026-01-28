import { AppRouter } from "@/app/router"
import { AuthInitializer } from "@/features/auth/AuthInitializer"
export default function App() {
    return (
        <>
            <AuthInitializer />
            <AppRouter />
        </>
    )
}