import { useLoginMutation } from '../services/authApi'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { loginSchema } from '../schemas/loginSchema'
import type { LoginSchema } from '../schemas/loginSchema'
import { useAppDispatch } from '../store/hooks'
import { setAuthenticated } from '../store/authSlice'
import { useNavigate } from 'react-router-dom'
import {
    TextField,
    Button,
    Box,
    Typography,
    Paper,
  
} from '@mui/material'
import { Link } from 'react-router-dom'

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
            <Paper sx={{ p: 4, width: '100%', maxWidth: 400 }}>
                <Typography variant="h4" textAlign="center" mb={2}>
                    Login
                </Typography>

                <Box
                    component="form"
                    onSubmit={handleSubmit(onSubmit)}
                    sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}
                >
                    <TextField
                        label="Email"
                        {...register('email')}
                        error={!!errors.email}
                        helperText={errors.email?.message}
                    />

                    <TextField
                        label="Password"
                        type="password"
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
                </Box>
                <Typography variant="body2" textAlign="center" sx={{ mt: 2 }}>
                    Don't have an account? <Link to="/register">Create one</Link>
                </Typography>
         
            </Paper>
        </Box>
            )
}
