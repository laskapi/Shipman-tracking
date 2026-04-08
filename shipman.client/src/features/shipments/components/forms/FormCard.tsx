import { Card, CardHeader, CardContent } from "@mui/material";
import { formCardContentSx } from "@/ui/formSpacing";

interface Props
{
    title: string;
    children: React.ReactNode;
}

export function FormCard({ title, children }: Props)
{
    return (
        <Card>
        <CardHeader title= { title } />
        <CardContent sx={formCardContentSx}>{children}</CardContent>
        </Card>
  );
}
