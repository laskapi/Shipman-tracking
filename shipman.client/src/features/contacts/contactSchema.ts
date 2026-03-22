import { z } from "zod";

const phoneRegex = /^\+?[0-9\s\-]{7,20}$/;

// Schema for creating a contact
export const createContactSchema = z.object({
    name: z.string().min(1, "Name is required"),
    email: z.string().email("Invalid email"),
    phone: z.string().regex(phoneRegex, "Invalid phone number"),
    addressId: z.string().nullable()
});

export type CreateContactDto = z.infer<typeof createContactSchema>;
