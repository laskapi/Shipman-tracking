import type { SxProps, Theme } from "@mui/material/styles"

/** Vertical spacing between form fields (MUI spacing scale, matches shipment forms). */
export const FORM_STACK_SPACING = 2

/** Card body padding for full-page forms (create / edit shipment). */
export const formCardContentSx: SxProps<Theme> = {
    py: 1.5,
    px: 2,
    "&:last-child": { pb: 1.5 },
}
