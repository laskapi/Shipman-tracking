import { Card, CardHeader, CardContent } from "@mui/material";

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
        <CardContent>{ children } </CardContent>
        </Card>
  );
}
