import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'

import { Store } from '../types/appTypes';
import { ApiError, ApiListData } from '../types/apiTypes';
import { StorePatchFormFields, StorePostFormFields } from '../types/formTypes';
import { storesApi } from '../api/storesApi';

const sliceName = 'store';

// /---------------------------------------- THUNKS ----------------------------------------\

export const getAllStore = createAsyncThunk<
    ApiListData<Store>,
    undefined,
    { rejectValue: ApiError }
>(
    `${sliceName}/getStore`,
    async (_, thunkAPI) => {
        return storesApi.getAll()
            .then((res) => thunkAPI.fulfillWithValue(res as ApiListData<Store>))
            .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
    }
);

export const getStore = createAsyncThunk<
    Store,
    number,
    { rejectValue: ApiError }
>(
    `${sliceName}/getStore`,
    async (payload, thunkAPI) => {
        return storesApi.get(payload)
            .then((res) => thunkAPI.fulfillWithValue(res as Store))
            .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
    }
);

export const createStore = createAsyncThunk<
    Store,
    StorePostFormFields,
    { rejectValue: ApiError }
>(
    `${sliceName}/createStore`,
    async (payload, thunkAPI) => {
        return storesApi.create(payload)
            .then((res) => thunkAPI.fulfillWithValue(res as Store))
            .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
    }
);

export const updateStore = createAsyncThunk<
    Store,
    { storeId: number, newData: StorePatchFormFields },
    { rejectValue: ApiError }
>(
    `${sliceName}/updateStore`,
    async (payload, thunkAPI) => {
        return storesApi.patch(payload.storeId, payload.newData)
            .then((res) => thunkAPI.fulfillWithValue(res as Store))
            .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
    }
);

export const deleteStore = createAsyncThunk<
    undefined,
    number,
    { rejectValue: ApiError }
>(
    `${sliceName}/deleteStore`,
    async (payload, thunkAPI) => {
        return storesApi.delete(payload)
            .then((res) => thunkAPI.fulfillWithValue(undefined))
            .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
    }
);


// \---------------------------------------- THUNKS ----------------------------------------/


interface StoreState {
    isLoading: boolean;
    currentStore?: Store;
    error?: ApiError;
}


const initialState: StoreState = {
    isLoading: false,
};

export const storeReducer = createSlice({
    name: sliceName,
    initialState: initialState,
    reducers: {

    },
    extraReducers: builder => {
        builder
            // getStore
            .addCase(getStore.pending, (state, action) => {
                state.isLoading = true;
                state.error = undefined;
            })
            .addCase(getStore.fulfilled, (state, { payload }) => {
                state.isLoading = false;
                state.currentStore = payload;
            })
            .addCase(getStore.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload ?? { message: 'Error. ' };
            })

            // createStore
            .addCase(createStore.pending, (state, action) => {
                state.isLoading = true;
                state.error = undefined;
            })
            .addCase(createStore.fulfilled, (state, { payload }) => {
                state.isLoading = false;
                state.currentStore = payload;
            })
            .addCase(createStore.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload ?? { message: 'Error. ' };
            })

            // updateStore
            .addCase(updateStore.pending, (state, action) => {
                state.isLoading = true;
                state.error = undefined;
            })
            .addCase(updateStore.fulfilled, (state, { payload }) => {
                state.isLoading = false;
                state.currentStore = payload;
            })
            .addCase(updateStore.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload ?? { message: 'Error. ' };
            })

            // deleteStore
            .addCase(deleteStore.pending, (state, action) => {
                state.isLoading = true;
                state.error = undefined;
            })
            .addCase(deleteStore.fulfilled, (state, action) => {
                state.isLoading = false;
                //state.currentStore = payload;
            })
            .addCase(deleteStore.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload ?? { message: 'Error. ' };
            })
    }
})

// export const { } = storeReducer.actions

export default storeReducer.reducer