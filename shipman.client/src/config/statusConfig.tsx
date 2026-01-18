type StatusColor =
    | "default"
    | "primary"
    | "secondary"
    | "error"
    | "info"
    | "success"
    | "warning"

export interface StatusUiConfig {
    color: StatusColor
    label: string
}
export const statusConfig: Record<string, StatusUiConfig> = {
    processing: { color: "info", label: "Processing" },
    shipped: { color: "primary", label: "Shipped" },
    intransit: { color: "warning", label: "In Transit" },
    delayed: { color: "error", label: "Delayed" },
    delivered: { color: "success", label: "Delivered" },
}
