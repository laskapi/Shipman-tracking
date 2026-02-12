import theme from "@/theme/theme"
import { ThemeProvider } from "@mui/material/styles"
import type { ReactNode } from "react"
import { Provider } from "react-redux"
import { BrowserRouter } from "react-router-dom"
import { store } from "./store"

interface ProvidersProps {
    children: ReactNode
}

export function Providers({ children }: ProvidersProps) {
    return (
        <Provider store={store}>
            <BrowserRouter>
                <ThemeProvider theme={theme}>
                    {children}
                </ThemeProvider>
            </BrowserRouter>
        </Provider>
    )
}
