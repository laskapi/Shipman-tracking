import { Box, Button } from "@mui/material";

interface Props
{
    isValid: boolean;
    isLoading: boolean;
    label: string;
}

export function FormAction({ isValid, isLoading, label }: Props)
{
    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "column",
                flexGrow: 1,
                justifyContent: "center",
                py: 2,
            }}
        >
            <Button
                variant="contained"
                type="submit"
                disabled={!isValid || isLoading}
                size="large"
            >
                {isLoading ? `${label}...` : label}
            </Button>
        </Box>
    );
}
