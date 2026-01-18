import { api } from './api'

export interface User {
    id: number
    email: string
}

export const authApi = api.injectEndpoints({
    endpoints: builder => ({
        login: builder.mutation<void, { email: string; password: string }>({
            query: body => ({
                url: 'auth/login',
                method: 'POST',
                body
            })
            // backend should set HttpOnly cookie here
        }),

        logout: builder.mutation<void, void>({
            query: () => ({
                url: 'auth/logout',
                method: 'POST'
            })
            // backend clears cookie here
        }),

        register: builder.mutation<void, { email: string; password: string }>({
            query: body => ({
                url: "auth/register",
                method: "POST",
                body
            })
        }),


        getProfile: builder.query<User, void>({
            query: () => 'auth/me'
        })
    })
})

export const {
    useLoginMutation,
    useLogoutMutation,
    useGetProfileQuery,
    useRegisterMutation
} = authApi
