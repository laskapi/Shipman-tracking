import { zodResolver } from '@hookform/resolvers/zod'
import {
    Box,
    Button,
    Paper,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { FORM_STACK_SPACING } from '@/ui/formSpacing'
import { useForm } from 'react-hook-form'
import { Link, useNavigate } from 'react-router-dom'
import type { LoginSchema } from './schemas/loginSchema'
import { loginSchema } from './schemas/loginSchema'
import { useLoginMutation } from '@/features/auth/authApi'
import { setAuthenticated } from '@/features/auth/authSlice'
import { useAppDispatch } from '@/app/storeHooks'

export default function Login() {
    const [login, { isLoading, error }] = useLoginMutation()
    const dispatch = useAppDispatch()
    const navigate = useNavigate()
    const {
        register,
        handleSubmit,
        formState: { errors }
    } = useForm<LoginSchema>({
        resolver: zodResolver(loginSchema)
    })

    const onSubmit = async (data: LoginSchema) => {
        await login(data).unwrap()
        dispatch(setAuthenticated())
        navigate("/")
        
    }

    return (
        <Box
            sx={{
                minHeight: '100vh',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                
            }}
        >
            <Paper sx={{ px: 2, py: 2, width: '100%', maxWidth: 400 }}>
                <Typography variant="h4" textAlign="center" mb={2}>
                    Login
                </Typography>

                <Stack
                    component="form"
                    spacing={FORM_STACK_SPACING}
                    onSubmit={handleSubmit(onSubmit)}
                >
                    <TextField
                        label="Email"
                        type="email"
                        autoComplete="email"
                        {...register('email')}
                        error={!!errors.email}
                        helperText={errors.email?.message}
                    />

                    <TextField
                        label="Password"
                        type="password"
                        autoComplete="current-password"
                        {...register('password')}
                        error={!!errors.password}
                        helperText={errors.password?.message}
                    />

                    <Button type="submit" variant="contained" disabled={isLoading}>
                        Login
                    </Button>

                    {error && (
                        <Typography color="error" textAlign="center">
                            Login failed
                        </Typography>
                    )}
                </Stack>
                <Typography variant="body2" textAlign="center" sx={{ mt: 2 }}>
                    Don't have an account? <Link to="/register">Create one</Link>
                </Typography>
         
            </Paper>
        </Box>
            )
}
