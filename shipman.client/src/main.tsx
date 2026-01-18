import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import theme from './theme.ts'
import { ThemeProvider } from '@mui/material/styles'
import { store } from './store/store.ts'
import { Provider } from 'react-redux'


createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ThemeProvider theme={theme}>
            <Provider store={store }>
                <App />
            </Provider>
            </ThemeProvider>
   </StrictMode>
)
