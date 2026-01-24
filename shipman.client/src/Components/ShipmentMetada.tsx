import { Paper, Typography, Grid } from "@mui/material";
import type { ShipmentDetails } from "@/types/shipment";

export default function ShipmentMetadata({ shipment }: { shipment: ShipmentDetails }) {
    return (
        <Paper
            elevation={1}
            sx={{
                p: 3,
                borderRadius: 3,
                mb: 3,
            }}
        >
            <Typography variant="h6" mb={3} fontWeight={600}>
                Additional Details
            </Typography>

            <Grid container spacing={2}>
                <Grid size={{ xs: 12, sm: 6 }}>
                    <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
                        Last Updated
                    </Typography>
                    <Typography variant="body1" fontWeight={500}>
                        {new Date(shipment.updatedAt).toLocaleString()}
                    </Typography>
                </Grid>

                {shipment.estimatedDelivery && (
                    <Grid size={{ xs: 12, sm: 6 }}>
                        <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
                            Estimated Delivery
                        </Typography>
                        <Typography variant="body1" fontWeight={500}>
                            {new Date(shipment.estimatedDelivery).toLocaleString()}
                        </Typography>
                    </Grid>
                )}
            </Grid>
        </Paper>
    );
}
