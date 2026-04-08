import { z } from 'zod';

const houseNumberPattern = /^[0-9A-Za-z\-/.]+$/;
const postalPattern = /^[0-9A-Za-z\s\-]+$/;

export const createAddressSchema = z.object({
    street: z.string().trim().min(1, 'Street is required'),
    houseNumber: z
        .string()
        .trim()
        .min(1, 'House number is required')
        .regex(
            houseNumberPattern,
            'Use letters, numbers, or common symbols (e.g. 12, 12A, 12/14)'
        ),
    apartmentNumber: z.string().optional(),
    city: z.string().trim().min(1, 'City is required'),
    postalCode: z
        .string()
        .trim()
        .min(2, 'Postal code is required')
        .regex(postalPattern, 'Postal code looks invalid'),
    country: z.string().trim().min(1, 'Country is required')
});

export type CreateAddressFormValues = z.infer<typeof createAddressSchema>;
