import { Button, TextField, MenuItem, Paper, FormControl, InputLabel, Select } from "@mui/material"
import ClearIcon from "@mui/icons-material/Clear"
import RefreshIcon from "@mui/icons-material/Refresh"
import type { ShipmentStatusDto } from "../types"
interface Props {
    statuses: ShipmentStatusDto[]
    statusFilter: string
    setStatusFilter: (value: string) => void
    search: string
    setSearch: (value: string) => void
    onClear: () => void
    onRefresh: () => void
}

export function ShipmentsToolbar({
    statuses,
    statusFilter,
    setStatusFilter,
    search,
    setSearch,
    onClear,
    onRefresh
}: Props) {
    return (
        <Paper
            elevation={1}
            sx={{
                p: 2,
                mb: 2,
                display: "flex",
                gap: 2,
                alignItems: "center",
                borderRadius: 2
            }}
        >
            <FormControl size="small" sx={{ width: 200 }}>
                <InputLabel shrink>Status</InputLabel>

                <Select
                    label="Status"
                    value={statusFilter}
                    onChange={(e) => setStatusFilter(e.target.value)}
                    displayEmpty
                    renderValue={(value: string) =>
                        value === "" ? "All statuses" : statuses.find(s=>s.value===value)?.label
                    }
                >
                    <MenuItem value="">All statuses</MenuItem>

                    {statuses.map((s) => (
                        <MenuItem key={s.value} value={s.value}>
                            {s.label}
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
            <TextField
                label="Search tracking number"
                value={search}
                onChange={(e) => setSearch(e.target.value)}
                size="small"
                fullWidth
            />
            <Button
                variant="outlined"
                color="warning"
                onClick={onClear}
                startIcon={<ClearIcon />}
            >
                Clear
            </Button>
            <Button
                variant="contained"
                onClick={onRefresh}
                startIcon={<RefreshIcon />}
            >
                Refresh
            </Button>
        </Paper>
    )
}
