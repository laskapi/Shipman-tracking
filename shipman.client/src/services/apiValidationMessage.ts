/** First human-readable message from our API `{ errors: Record<string, string[]> }` shape (400 responses). */
export function firstApiValidationMessage(err: unknown): string | null
{
    if (!err || typeof err !== 'object' || !('data' in err)) return null;
    const data = (err as { data: unknown }).data;
    if (!data || typeof data !== 'object' || !('errors' in data)) return null;
    const errors = (data as { errors: Record<string, string[] | undefined> }).errors;
    if (!errors || typeof errors !== 'object') return null;

    const general = errors.General ?? errors.general;
    if (Array.isArray(general) && general[0]) return general[0];

    for (const key of ['PrimaryAddress', 'primaryAddress', 'Address', 'address'] as const)
    {
        const v = errors[key];
        if (Array.isArray(v) && v[0]) return v[0];
    }

    const flat = Object.values(errors)
        .flat()
        .filter((x): x is string => typeof x === 'string' && x.length > 0);
    return flat[0] ?? null;
}
