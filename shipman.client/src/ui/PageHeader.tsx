import { AppBar, Breadcrumbs, Stack, Toolbar, Typography } from "@mui/material"
import { Link as RouterLink } from "react-router-dom"

interface PageHeaderProps {
    title: string
    subtitle?: string
    breadcrumb?: Array<{ label: string; to?: string }>
    actions?: React.ReactNode
}

export function PageHeader({ title, subtitle, breadcrumb, actions }: PageHeaderProps) {
   
    return (
        <AppBar
            id="app-header"
            position="static"
            color="default"
            elevation={1}
            sx={{
                backgroundColor: "rgba(0,0,0,0.02)",
                boxShadow: "0 1px 2px rgba(0,0,0,0.08)"
            }}
        >
            <Toolbar
                sx={{
                    flexDirection: "column",
                    alignItems: "flex-start",
                    width: "100%",
             
                }}
            >
                {breadcrumb && (
                    <Breadcrumbs sx={{ mb: 1 }}>
                        {breadcrumb.map((item, i) =>
                            item.to ? (
                                <RouterLink
                                    key={i}
                                    to={item.to}
                                    style={{ textDecoration: "none" }}
                                >
                                    <Typography color="text.secondary">{item.label}</Typography>
                                </RouterLink>
                            ) : (
                                <Typography key={i} color="text.primary">
                                    {item.label}
                                </Typography>
                            )
                        )}
                    </Breadcrumbs>
                )}

                <Stack
                    direction={{ xs: "column", md: "row" }}
                    justifyContent="space-between"
                    alignItems={{ xs: "flex-start", md: "center" }}
                    spacing={2}
                    sx={{ width: "100%" }}
                >
                    <Stack direction="row" spacing={1} alignItems="center" flexWrap="wrap">
                        <Typography variant="h4" color="text.primary">
                            {title}
                        </Typography>

                        {subtitle && (
                            <Stack direction="row" spacing={1} alignItems="center">
                                <Typography component="span" color="text.secondary">
                                    ●
                                </Typography>
                                <Typography
                                    component="span"
                                    variant="h6"
                                    color="text.secondary"
                                    sx={{ fontWeight: 400 }}
                                >
                                    {subtitle}
                                </Typography>
                            </Stack>
                        )}
                    </Stack>

                    {actions && (
                        <Stack
                            direction="row"
                            spacing={1}
                            alignItems="center"
                            sx={{
                                flexWrap: "wrap",
                                rowGap: 1,
                                width: { xs: "100%", md: "auto" },
                                justifyContent: { xs: "flex-start", md: "flex-end" }
                            }}
                        >
                            {actions}
                        </Stack>
                    )}
                </Stack>
            </Toolbar>
        </AppBar>
    )
}
