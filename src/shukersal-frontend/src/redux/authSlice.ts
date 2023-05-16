import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'

import { localStorageValues } from '../_configuration';
import { authApi } from '../api/authApi';
import { Member } from '../types/appTypes';
import { ApiError } from '../types/apiTypes';
import { LoginFormFields, RegisterFormFields } from '../types/formTypes';

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

    export const register = createAsyncThunk<
    Member,
    RegisterFormFields,
    { rejectValue: ApiError }
>(
    `${sliceName}/register`,
    async (payload, thunkAPI) => {
        const { confirmPassword, ...credentials } = payload;
        return authApi.register(credentials)
            .then((res) => thunkAPI.fulfillWithValue(res as Member))
            .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
    });

export const logout = createAsyncThunk<
    undefined,
    undefined,
    { rejectValue: ApiError }
>(
    `${sliceName}/logout`,
    async (payload, thunkAPI) => {
        return thunkAPI.fulfillWithValue(undefined);
        // TODO: Implement once notificaitons sockets are implemented
        // return notificationsApi.disconnect()
        //     .then((res) => thunkAPI.fulfillWithValue(undefined))
        //     .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
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


const localStorageData = (() => {
    try {
        return JSON.parse(window.localStorage.getItem(localStorageValues.auth.currentMemberData.name) ?? '')
    } catch (err) {
        console.error("Can't parse data: ", err);
        return undefined;
    }
})();
console.log("[DEBUG] ", localStorageData);
const initialState: AuthState = (localStorageData?.member && localStorageData?.token)
    ? {
        isLoading: false,
        data: {
            token: localStorageData.token,
            currentMember: localStorageData.member
        }
    }
    : {
        isLoading: false,
    };

export const authSlice = createSlice({
    name: sliceName,
    initialState: initialState,
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
                state.isLoading = false;
                state.data = {
                    token: payload.token,
                    currentMember: payload.member
                }
                if (payload.rememberMe) {
                    window.localStorage.setItem(localStorageValues.auth.currentMemberData.name, JSON.stringify(payload));
                } else {
                    window.localStorage.removeItem(localStorageValues.auth.currentMemberData.name);
                }
            })
            .addCase(login.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload ?? { message: 'Login Error. ' };
            })

            // logout
            .addCase(logout.pending, (state, action) => {
                state.isLoading = true;
                state.error = undefined;
            })
            .addCase(logout.fulfilled, (state, { payload }) => {
                state.isLoading = false;
                state.data = undefined;
                window.localStorage.removeItem(localStorageValues.auth.currentMemberData.name);

            })
            .addCase(logout.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload ?? { message: 'Logout Error. ' };
            })
    }
})

export const { } = authSlice.actions

export default authSlice.reducer