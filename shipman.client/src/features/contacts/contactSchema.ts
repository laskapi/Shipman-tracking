import { z } from 'zod';
import { createAddressSchema } from '@/features/addresses/addressSchema';

const phoneRegex = /^\+?[0-9\s\-]{7,20}$/;

export const createContactSchema = z.object({
    name: z.string().trim().min(1, 'Name is required'),
    email: z.string().trim().email('Invalid email'),
    phone: z
        .string()
        .trim()
        .regex(phoneRegex, 'Use 7–20 digits; optional + and spaces/dashes'),
    primaryAddress: createAddressSchema
});

export type CreateContactFormValues = z.infer<typeof createContactSchema>;
