import { z } from "zod";
export const editShipmentSchema = z.object({
    receiver: z.object({
        name: z.string().min(1, "Name is required"),
        email: z.string().email("Invalid email"),
        phone: z.string().min(3, "Phone is too short"),
        address: z.string().min(3, "Address is too short")
    }),
    serviceType: z.enum(["Standard", "Express", "Freight"])
});

export type EditShipmentDto = z.infer<typeof editShipmentSchema>;