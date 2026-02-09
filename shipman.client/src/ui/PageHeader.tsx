interface PageHeaderProps {
    title: string
    subtitle?: string
    breadcrumb?: Array<{ label: string; to?: string }>
    actions?: React.ReactNode
}

import { Box, Stack, Typography, Breadcrumbs } from "@mui/material"
import { Link as RouterLink } from "react-router-dom"

export function PageHeader({ title, subtitle, breadcrumb, actions }: PageHeaderProps) {
        
    return (
        <Box mb={3}>
            <Box sx={{ minHeight: 24, display: "flex", alignItems: "center" }}>
            
            {breadcrumb && (
                <Breadcrumbs sx={{ mb: 1 }}>
                    {breadcrumb.map((item, i) =>
                        item.to ? (
                            <RouterLink key={i} to={item.to} style={{ textDecoration: "none" }}>
                                <Typography color="primary">{item.label}</Typography>
                            </RouterLink>
                        ) : (
                            <Typography key={i} color="text.primary">
                                {item.label}
                            </Typography>
                        )
                    )}
                </Breadcrumbs>
                )}
            </Box>

            <Stack direction="row" justifyContent="space-between" alignItems="center">

                <Box>
                    <Typography variant="h4" sx={{ display: "flex", alignItems: "center", gap: 1 }}>
                        {title}
                        {subtitle && (
                            <Typography
                                component="span"
                                variant="h6"
                                color="text.secondary"
                                sx={{ fontWeight: 400 }}
                            >
                                {"\u25CF"} {subtitle}
                            </Typography>
                        )}
                    </Typography>

                </Box>

                {actions && (
                    <Stack direction="row" spacing={1}>
                        {actions}
                    </Stack>
                )}
            </Stack>
        </Box>
    )
}
