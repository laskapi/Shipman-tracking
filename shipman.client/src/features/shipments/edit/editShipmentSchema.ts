import { z } from "zod";

// Schema for editing an existing shipment
export const editShipmentSchema = z
    .object({
        receiverId: z.string().min(1, "Receiver is required"),
        destinationAddressId: z.string().nullable(),
        serviceType: z.enum(["Standard", "Express", "Freight"])
    })
    .superRefine((data, ctx) =>
    {
        if (
            data.receiverId &&
            (data.destinationAddressId == null ||
                data.destinationAddressId === "")
        )
        {
            ctx.addIssue({
                code: z.ZodIssueCode.custom,
                message: "Destination address is required",
                path: ["destinationAddressId"]
            });
        }
    });

export type EditShipmentDto = z.infer<typeof editShipmentSchema>;
