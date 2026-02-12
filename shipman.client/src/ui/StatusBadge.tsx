import { Chip } from "@mui/material"
import { statusConfig } from "../config/statusConfig"
export default function StatusBadge({ status }: { status: string }) {
    const key = status.toLowerCase()
    const cfg = statusConfig[key]

    if (!cfg) {
        return <Chip label={status} size="small" />
    }
    return (
        <Chip
            label={cfg.label}
            color={cfg.color}
            size="small"
            sx={{ fontWeight: 600 }}
        />
    )
}
