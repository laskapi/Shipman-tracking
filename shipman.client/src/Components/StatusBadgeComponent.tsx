import { Chip } from "@mui/material"
import { statusConfig } from "../config/statusConfig"

export function StatusBadge({ status }: { status: string }) {
    // Normalize backend value: "Processing" → "processing", "InTransit" → "intransit"
    const key = status.toLowerCase()
    const cfg = statusConfig[key]

    // If backend adds a new status we haven't styled yet
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
