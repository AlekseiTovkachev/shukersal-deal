import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'

import { Product, Store } from '../types/appTypes';
import { ApiError, ApiListData } from '../types/apiTypes';
import { ProductPostFormFields, StorePatchFormFields, StorePostFormFields } from '../types/formTypes';
import { storeProductsApi, storesApi } from '../api/storesApi';

const sliceName = 'store';

// /---------------------------------------- THUNKS ----------------------------------------\

export const getAllStore = createAsyncThunk<
    ApiListData<Store>,
    undefined,
    { rejectValue: ApiError }
>(
    `${sliceName}/getAllStore`,
    async (_, thunkAPI) => {
        return storesApi.getAll()
            .then((res) => thunkAPI.fulfillWithValue(res as ApiListData<Store>))
            .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
    }
);

export const getMyStores = createAsyncThunk<
    ApiListData<Store>,
    number,
    { rejectValue: ApiError }
>(
    `${sliceName}/getMyStores`,
    async (memberId, thunkAPI) => {
        return storesApi.getMyStores(memberId)
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

export const createProduct = createAsyncThunk<
    Product,
    {storeId: number, product: ProductPostFormFields},
    { rejectValue: ApiError }
    >(
`${sliceName}/createProduct`,
async (payload, thunkAPI) => {
    return storeProductsApi.create(payload.storeId, payload.product)
        .then((res) => thunkAPI.fulfillWithValue(res as Product))
        .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
    }
);

export const editProduct = createAsyncThunk<
    Product,
    {storeId: number, productId: number, product: Partial<Product>},
    { rejectValue: ApiError }
    >(
`${sliceName}/editProduct`,
async (payload, thunkAPI) => {
    return storeProductsApi.patch(payload.storeId, payload.productId, payload.product)
        .then((res) => thunkAPI.fulfillWithValue(res as Product))
        .catch((res) => thunkAPI.rejectWithValue(res as ApiError))
    }
);

// \---------------------------------------- THUNKS ----------------------------------------/


interface StoreState {
    isLoading: boolean;
    myStores: ApiListData<Store>;
    currentStore?: Store;
    error?: ApiError;
}


const initialState: StoreState = {
    isLoading: false,
    myStores: []
};

export const storeReducer = createSlice({
    name: sliceName,
    initialState: initialState,
    reducers: {
        resetCurrentStore: (state) => {
            state.currentStore = undefined;
        }
    },
    extraReducers: builder => {
        builder
            // getMyStores
            .addCase(getMyStores.pending, (state, action) => {
                state.isLoading = true;
                state.error = undefined;
            })
            .addCase(getMyStores.fulfilled, (state, { payload }) => {
                state.isLoading = false;
                state.myStores = payload;
            })
            .addCase(getMyStores.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload ?? { message: 'Error. ' };
            })

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

            // createProduct
            .addCase(createProduct.pending, (state, action) => {
                state.isLoading = true;
                state.error = undefined;
            })
            .addCase(createProduct.fulfilled, (state, action) => {
                state.isLoading = false;
                //state.currentStore = payload;
            })
            .addCase(createProduct.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload ?? { message: 'Error. ' };
            })

            // editProduct
            .addCase(editProduct.pending, (state, action) => {
                state.isLoading = true;
                state.error = undefined;
            })
            .addCase(editProduct.fulfilled, (state, action) => {
                state.isLoading = false;
                //state.currentStore = payload;
            })
            .addCase(editProduct.rejected, (state, action) => {
                state.isLoading = false;
                state.error = action.payload ?? { message: 'Error. ' };
            })
    }
})

export const { resetCurrentStore } = storeReducer.actions

export default storeReducer.reducer