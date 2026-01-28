import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import type { RegisterSchema } from "./schemas/registerSchema"
import { registerSchema } from "./schemas/registerSchema"
import { useRegisterMutation } from "@/features/auth/authApi"

import {
    Box,
    Button,
    Paper,
    TextField,
    Typography
} from "@mui/material"
import { Link, useNavigate } from "react-router-dom"

export default function Register() {
    const [registerUser, { isLoading, error }] = useRegisterMutation()
    const navigate = useNavigate()

    const {
        register,
        handleSubmit,
        formState: { errors }
    } = useForm<RegisterSchema>({
        resolver: zodResolver(registerSchema)
    })

    const onSubmit = async (data: RegisterSchema) => {
        await registerUser({
            email: data.email,
            password: data.password
        }).unwrap()

        // backend sets cookie OR you redirect to login
        navigate("/login")
    }

    return (
        <Box
            sx={{
                minHeight: "100vh",
                display: "flex",
                alignItems: "center",
                justifyContent: "center"
            }}
        >
            <Paper sx={{ p: 4, width: "100%", maxWidth: 400 }}>
                <Typography variant="h4" textAlign="center" mb={2}>
                    Register
                </Typography>

                <Box
                    component="form"
                    onSubmit={handleSubmit(onSubmit)}
                    sx={{ display: "flex", flexDirection: "column", gap: 2 }}
                >
                    <TextField
                        label="Email"
                        {...register("email")}
                        error={!!errors.email}
                        helperText={errors.email?.message}
                    />

                    <TextField
                        label="Password"
                        type="password"
                        {...register("password")}
                        error={!!errors.password}
                        helperText={errors.password?.message}
                    />

                    <TextField
                        label="Confirm Password"
                        type="password"
                        {...register("confirmPassword")}
                        error={!!errors.confirmPassword}
                        helperText={errors.confirmPassword?.message}
                    />

                    <Button type="submit" variant="contained" disabled={isLoading}>
                        Register
                    </Button>

                    {error && (
                        <Typography color="error" textAlign="center">
                            Registration failed
                        </Typography>
                    )}
                </Box>
                <Typography variant="body2" textAlign="center" sx={{ mt: 2 }}>
                    Already have an account? <Link to="/login">Log in</Link>
                </Typography>

            </Paper>
        </Box>
    )
}




/*import React from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import {
    Box,
    Button,
    TextField,
    Typography,
    Container,
    Paper,
    Avatar,
} from "@mui/material";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import { Link } from "react-router-dom";

// Zod schema
const registerSchema = z
    .object({
        username: z
            .string()
            .min(6, "Username must be at least 6 characters"),
        email: z
            .string()
            .email("Invalid email address"),
        password: z
            .string()
            .min(6, "Password must be at least 6 characters"),
        confirmPassword: z
            .string()
            .min(6, "Password must be at least 6 characters"),
    })
    .refine((data) => data.password === data.confirmPassword, {
        message: "Passwords don't match",
        path: ["confirmPassword"],
    });

// Infer TypeScript type
type RegisterFormValues = z.infer<typeof registerSchema>;

const Register: React.FC = () => {
    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
    } = useForm<RegisterFormValues>({
        resolver: zodResolver(registerSchema),
        defaultValues: {
            username: "",
            email: "",
            password: "",
            confirmPassword: "",
        },
    });

    const onSubmit = async (data: RegisterFormValues) => {
        console.log("Register Data:", data);
        await new Promise((resolve) => setTimeout(resolve, 1000));
    };

    return (
        <Container component="main" maxWidth="xs">
            <Paper
                elevation={3}
                sx={{
                    mt: 8,
                    p: 4,
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "center",
                }}
            >
                <Avatar sx={{ m: 1, bgcolor: "secondary.main" }}>
                    <LockOutlinedIcon />
                </Avatar>

                <Typography component="h1" variant="h5">
                    Sign Up
                </Typography>

                <Box
                    component="form"
                    onSubmit={handleSubmit(onSubmit)}
                    noValidate
                    sx={{ mt: 1, width: "100%" }}
                >
                    <TextField
                        margin="normal"
                        fullWidth
                        label="Username"
                        autoComplete="username"
                        {...register("username")}
                        error={!!errors.username}
                        helperText={errors.username?.message}
                    />

                    <TextField
                        margin="normal"
                        fullWidth
                        label="Email Address"
                        autoComplete="email"
                        {...register("email")}
                        error={!!errors.email}
                        helperText={errors.email?.message}
                    />

                    <TextField
                        margin="normal"
                        fullWidth
                        label="Password"
                        type="password"
                        autoComplete="new-password"
                        {...register("password")}
                        error={!!errors.password}
                        helperText={errors.password?.message}
                    />

                    <TextField
                        margin="normal"
                        fullWidth
                        label="Confirm Password"
                        type="password"
                        autoComplete="new-password"
                        {...register("confirmPassword")}
                        error={!!errors.confirmPassword}
                        helperText={errors.confirmPassword?.message}
                    />

                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{ mt: 3, mb: 2 }}
                        disabled={isSubmitting}
                    >
                        {isSubmitting ? "Signing Up..." : "Sign Up"}
                    </Button>
                </Box>

                <Typography variant="body2">
                    Already have an account?{" "}
                    <Link to="/login">Sign in.</Link>
                </Typography>
            </Paper>
        </Container>
    );
};

export default Register;*/