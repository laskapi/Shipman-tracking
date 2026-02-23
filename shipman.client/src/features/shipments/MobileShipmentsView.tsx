import { Box, Paper, Typography } from "@mui/material"
import { ShipmentsToolbar } from "./components/ShipmentsToolbar"
import type { ShipmentsController } from "./types"

export function MobileShipmentsView({ ctrl }: { ctrl: ShipmentsController }) {
    const { data, handleRowClick } = ctrl

    return (
        <Box
            sx={{
                display: { xs: "flex", md: "none" },
                flexDirection: "column",
                minHeight: 0,
                flexGrow: 1,
            }}
        >
            <Box
                sx={{
                    position: "sticky",
                    top: 0,
                    zIndex: 20,
                    backgroundColor: "background.paper",
                    pt: 1,
                    pb: 1,
                }}
            >
                <ShipmentsToolbar ctrl={ctrl.toolbar} />
            </Box>

            <Box
                sx={{
                    overflowY: "auto",
                    flexGrow: 1,
                    minHeight: 0,

                    px: 2,
                    pt: 1,
                    pb: 2,

                    display: "flex",
                    flexDirection: "column",
                    gap: 2,
                }}
            >
                {data?.items.map((s) => (
                    <Paper key={s.id}
                        sx={{ p: 2, boxShadow: 1 }}
                        onClick={() => handleRowClick({ id: s.id })}
                    >
                        <Typography variant="subtitle1" fontWeight={600}>
                            Shipment #{s.trackingNumber}
                        </Typography>

                        <Typography color="text.secondary">Origin: {s.origin}</Typography>
                        <Typography color="text.secondary">Destination: {s.destination}</Typography>
                        <Typography color="text.secondary">Status: {s.status}</Typography>
                        <Typography color="text.secondary">Last updated: {s.updatedAt}</Typography>

                    </Paper>
                ))}
            </Box>
        </Box>
    )
}
