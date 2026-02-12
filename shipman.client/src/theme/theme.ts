import { createTheme } from "@mui/material/styles";
import type { } from "@mui/x-data-grid/themeAugmentation";

const PRIMARY = "#3A6EA5";
const HEADER_BG = "#3A6EA5";
const HEADER_TEXT = "#F0F4F8";

const theme = createTheme({
    palette: {
        primary: {
            main: PRIMARY,
        },
    },

    typography: {
        button: {
            textTransform: "none",
        },
    },

    components: {
        MuiDataGrid: {
            styleOverrides: {
                root: ({ theme }) => ({
                    "& .MuiDataGrid-columnHeader": {
                        backgroundColor: HEADER_BG,
                        color: HEADER_TEXT,
                        fontWeight: 700,
                        letterSpacing: "0.3px",
                    },

                    "& .MuiDataGrid-sortIcon": {
                        color: HEADER_TEXT,
                    },

                    "& .MuiDataGrid-iconButtonContainer .MuiButtonBase-root": {
                        backgroundColor: "transparent !important",
                        color: HEADER_TEXT,
                    },

                    "& .MuiDataGrid-menuIconButton": {
                        color: HEADER_TEXT,
                    },

                    "& .MuiDataGrid-menuIconButton .MuiSvgIcon-root": {
                        color: HEADER_TEXT,
                    },

                    "& .MuiDataGrid-cell": {
                        fontSize: theme.typography.pxToRem(13),
                        [theme.breakpoints.down("sm")]: {
                            fontSize: theme.typography.pxToRem(12),
                        },
                    },
                }),

                columnHeaderTitle: ({ theme }) => ({
                    fontWeight: 700,
                    letterSpacing: "0.3px",

                    fontSize: theme.typography.pxToRem(15),
                    [theme.breakpoints.down("sm")]: {
                        fontSize: theme.typography.pxToRem(13),
                    },
                }),
            },
        },


        MuiButton: {
            styleOverrides: {
                root: {
                    borderRadius: 6,
                },
            },
        },

        MuiPaper: {
            styleOverrides: {
                root: {
                    borderRadius: 8,
                    padding: "16px",
                },
            },
        },
    },
});

export default theme;
