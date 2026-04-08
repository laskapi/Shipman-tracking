import { Typography } from "@mui/material";

export default function PanelHeader({ children }: { children: React.ReactNode })
{
    return (
        <Typography
            variant="subtitle1"
            sx={{
                pb: 2,
                fontWeight: 600,
            }}
        >
            {children}
        </Typography>
    );
}
