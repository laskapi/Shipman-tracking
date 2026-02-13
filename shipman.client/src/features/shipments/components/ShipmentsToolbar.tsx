import {
    Box,
    TextField,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Stack,
    Button
} from "@mui/material"
import ClearIcon from "@mui/icons-material/Clear"
import RefreshIcon from "@mui/icons-material/Refresh"
import type { ShipmentsToolbarController } from "../types"
import type { SelectChangeEvent } from "@mui/material"
export function ShipmentsToolbar({ ctrl }: { ctrl: ShipmentsToolbarController }) {
    return (
        <Box
            sx={{
                px: { xs: 2, md: 3 },
                py: { xs: 1, md: 1 },
                display: "flex",
                flexDirection: { xs: "column", md: "row" },
                gap: { xs: 1.5, md: 2 },
                alignItems: { xs: "stretch", md: "center" },
                backgroundColor: "background.paper",
            }}
        >
            <TextField
                label="Search tracking number"
                value={ctrl.search}
                onChange={(e) => ctrl.setSearch(e.target.value)}
                size="small"
                fullWidth
            />

            <FormControl size="small" sx={{ width: { xs: "100%", md: 200 } }}>
                <InputLabel shrink>Status</InputLabel>
                <Select
                    label="Status"
                    value={ctrl.status}
                    onChange={(e: SelectChangeEvent<string>) => ctrl.setStatus(e.target.value)}
                    displayEmpty
                    renderValue={(value) =>
                        value === ""
                            ? "All statuses"
                            : ctrl.statuses.find((s) => s.value === value)?.label
                    }
                >
                    <MenuItem value="">All statuses</MenuItem>
                    {ctrl.statuses.map((s) => (
                        <MenuItem key={s.value} value={s.value}>
                            {s.label}
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>

            <Stack
                direction={{ xs: "column", md: "row" }}
                spacing={1.5}
            >
                <Button
                    variant="outlined"
                    color="warning"
                    onClick={ctrl.clear}
                    startIcon={<ClearIcon />}
                >
                    Clear
                </Button>

                <Button
                    variant="contained"
                    onClick={ctrl.refresh}
                    startIcon={<RefreshIcon />}
                >
                    Refresh
                </Button>
            </Stack>
        </Box>
    )
}

