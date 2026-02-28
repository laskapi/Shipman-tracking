import { z } from "zod";

const phoneRegex = /^\+?[0-9\s\-]{7,20}$/;

const normalizeAddress = (value: string) =>
{
    const trimmed = value.trim();
    const collapsed = trimmed.replace(/\s+/g, " ");
    return collapsed;
};

export const contactSchema = z.object({
    name: z.string().min(1, "Name is required"),
    email: z.string().email("Invalid email"),
    phone: z
        .string()
        .min(1, "Phone is required")
        .regex(phoneRegex, "Invalid phone number"),
    address: z
        .string()
        .min(1, "Address is required")
        .transform(normalizeAddress),
});


export const createShipmentSchema = z.object({
    sender: contactSchema,
    receiver: contactSchema,
    weight: z
        .number({
            required_error: "Weight is required",
            invalid_type_error: "Weight must be a number",
        })
        .positive("Weight must be greater than 0"),
    serviceType: z.enum(["Standard", "Express", "Priority"]),
});

export type CreateShipmentDto = z.infer<typeof createShipmentSchema>;
