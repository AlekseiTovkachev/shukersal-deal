import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'

import { localStorageValues } from '../_configuration';
import { authApi } from '../api/authApi';
import { Member } from '../types/appTypes';
import { ApiError } from '../types/apiTypes';
import { LoginFormFields } from '../types/formTypes';

const sliceName = 'auth';

// /---------------------------------------- THUNKS ----------------------------------------\
export const login = createAsyncThunk<
    {
        member: Member,
        token: string,
        rememberMe: boolean
    },
    LoginFormFields,
    { rejectValue: ApiError }
>(
    `${sliceName}/login`,
    async (payload, thunkAPI) => {
        const { rememberMe, ...credentials } = payload;
        return authApi.login(credentials)
            .then((res) => thunkAPI.fulfillWithValue({
                member: res.member,
                token: res.token,
                rememberMe: rememberMe
            }))
            .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
    });
// \---------------------------------------- THUNKS ----------------------------------------/


interface AuthState {
    isLoading: boolean;
    data?: {
        token: string;
        currentMember: Member;
    };
    error?: ApiError;
}

// Define the initial state using that type
const initialState: AuthState = {
    isLoading: false,
}

export const authSlice = createSlice({
    name: sliceName,
    initialState,
    reducers: {
    },
    extraReducers: builder => {
        builder
            // login
            .addCase(login.pending, (state, action) => {
                state.isLoading = true;
                state.error = undefined;
            })
            .addCase(login.fulfilled, (state, { payload }) => {
                if (payload.rememberMe) {
                    window.localStorage.setItem(localStorageValues.auth.currentMemberData.name, JSON.stringify(payload));
                } else {
                    window.localStorage.removeItem(localStorageValues.auth.currentMemberData.name);
                }
            })
            .addCase(login.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.error.message ?? 'Login Error. ';
            })
    }
})

export const { } = authSlice.actions

export default authSlice.reducer