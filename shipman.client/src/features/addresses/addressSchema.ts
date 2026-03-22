import { z } from "zod";

// Schema for creating an address
export const createAddressSchema = z.object({
    street: z.string().min(1, "Street is required"),
    city: z.string().min(1, "City is required"),
    postalCode: z.string().min(1, "Postal code is required"),
    country: z.string().min(1, "Country is required")
});

export type CreateAddressDto = z.infer<typeof createAddressSchema>;
