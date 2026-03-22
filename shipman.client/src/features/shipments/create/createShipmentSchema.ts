import { z } from "zod";

// Schema for creating a shipment
export const createShipmentSchema = z.object({
    senderId: z.string().min(1, "Sender is required"),
    receiverId: z.string().min(1, "Receiver is required"),
    destinationAddressId: z.string().nullable(),

    weight: z
        .number({
            required_error: "Weight is required",
            invalid_type_error: "Weight must be a number"
        })
        .positive("Weight must be greater than 0"),

    serviceType: z.enum(["Standard", "Express", "Freight"])
});

export type CreateShipmentDto = z.infer<typeof createShipmentSchema>;
