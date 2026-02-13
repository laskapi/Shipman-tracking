import { createSlice, type PayloadAction } from "@reduxjs/toolkit"

export enum HeaderActionsType {
    None = "none",
    ShipmentsList = "shipmentsList",
    ShipmentDetails = "shipmentDetails",
    Dashboard = "dashboard"
}

export interface HeaderState {
    title: string
    subtitle?: string
    breadcrumb: Array<{ label: string; to?: string }>
    actionsType: HeaderActionsType
}

const initialState: HeaderState = {
    title: "",
    subtitle: "",
    breadcrumb: [],
    actionsType: HeaderActionsType.None
}

const headerSlice = createSlice({
    name: "header",
    initialState,
    reducers: {
        setHeader(state, action: PayloadAction<Partial<HeaderState>>) {
            return { ...state, ...action.payload }
        },
        clearHeader() {
            return initialState
        }
    }
})

export const { setHeader, clearHeader } = headerSlice.actions
export default headerSlice.reducer
