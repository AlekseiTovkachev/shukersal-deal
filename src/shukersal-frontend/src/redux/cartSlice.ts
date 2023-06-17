import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";

import { localStorageValues } from "../_configuration";
import {
  CartItem,
  Member,
  ShoppingCart,
  ShoppingItem,
} from "../types/appTypes";
import { ApiError } from "../types/apiTypes";
import { cartApi } from "../api/cartApi";
import { setLocalCart } from "./util";

const sliceName = "cart";

// /---------------------------------------- THUNKS ----------------------------------------\
export const getCart = createAsyncThunk<
  ShoppingCart,
  number,
  { rejectValue: ApiError }
>(`${sliceName}/getCart`, async (memberId, thunkAPI) => {
  return cartApi
    .getCart(memberId)
    .then((res) => thunkAPI.fulfillWithValue(res as ShoppingCart))
    .catch((res) => thunkAPI.rejectWithValue(res as ApiError));
});

export const addItem = createAsyncThunk<
  ShoppingItem,
  { cartId?: number; item: CartItem },
  { rejectValue: ApiError }
>(`${sliceName}/addItem`, async (payload, thunkAPI) => {
  if (payload.cartId)
    return cartApi
      .addItem(payload.cartId, payload.item)
      .then((res) => thunkAPI.fulfillWithValue(res as ShoppingItem))
      .catch((res) => thunkAPI.rejectWithValue(res as ApiError));

  return thunkAPI.fulfillWithValue({
    id: 0,
    shoppingBasketId: 0,
    ...payload.item,
  });
});

export const updateItem = createAsyncThunk<
  ShoppingItem,
  { cartId?: number; item: CartItem },
  { rejectValue: ApiError }
>(`${sliceName}/updateItem`, async (payload, thunkAPI) => {
  if (payload.cartId)
    return cartApi
      .updateItem(payload.cartId, payload.item)
      .then((res) => thunkAPI.fulfillWithValue(res as ShoppingItem))
      .catch((res) => thunkAPI.rejectWithValue(res as ApiError));
  return thunkAPI.fulfillWithValue({
    id: 0,
    shoppingBasketId: 0,
    ...payload.item,
  });
});

export const deleteItem = createAsyncThunk<
  ShoppingItem,
  { cartId?: number; productId: number },
  { rejectValue: ApiError }
>(`${sliceName}/deleteItem`, async (payload, thunkAPI) => {
  if (payload.cartId)
    return cartApi
      .deleteItem(payload.cartId, payload.productId)
      .then((res) => thunkAPI.fulfillWithValue(res as ShoppingItem))
      .catch((res) => thunkAPI.rejectWithValue(res as ApiError));
  return thunkAPI.fulfillWithValue({
    id: 0,
    quantity: 0,
    productId: payload.productId,
    shoppingBasketId: 0,
  });
});

// \---------------------------------------- THUNKS ----------------------------------------/

interface CartState {
  isLoading: boolean;
  cartId?: number;
  cartItems: CartItem[];
  error?: ApiError;
}

const localStorageCartItems = (() => {
  if(window.localStorage.getItem(localStorageValues.auth.currentMemberData.name)) return [];
  try {
    return JSON.parse(
      window.localStorage.getItem(localStorageValues.cartItems.name) ?? "[]"
    );
  } catch (err) {
    console.error("Can't parse data: ", err);
    return [];
  }
})();
const initialState: CartState = {
  isLoading: false,
  cartItems: localStorageCartItems,
};

export const cartSlice = createSlice({
  name: sliceName,
  initialState: initialState,
  reducers: {
    clearCart: (state) => {
      state.cartId = undefined;
      state.cartItems = [];
      setLocalCart([]);
    }
  },

  extraReducers: (builder) => {
    builder
      // getCart
      .addCase(getCart.pending, (state, action) => {
        state.isLoading = true;
        state.error = undefined;
      })
      .addCase(getCart.fulfilled, (state, { payload }) => {
        state.cartId = payload.id;
        const items: CartItem[] = [];
        payload.shoppingBaskets.forEach((basket) => {
          basket.shoppingItems.forEach((item) => {
            items.push({ productId: item.productId, quantity: item.quantity });
          });
        });
        state.cartItems = items;
        state.isLoading = false;
      })
      .addCase(getCart.rejected, (state, action) => {
        state.cartId = undefined;
        state.isLoading = false;
        state.error = action.payload ?? { message: "Error. " };
      })

      // addItem
      .addCase(addItem.pending, (state, action) => {
        state.isLoading = true;
        state.error = undefined;
      })
      .addCase(addItem.fulfilled, (state, { payload }) => {
        state.isLoading = false;
        state.cartItems.push({
          productId: payload.productId,
          quantity: payload.quantity,
        });
        setLocalCart(state.cartItems);
      })
      .addCase(addItem.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload ?? { message: "Error. " };
      })

      // updateItem
      .addCase(updateItem.pending, (state, action) => {
        state.isLoading = true;
        state.error = undefined;
      })
      .addCase(updateItem.fulfilled, (state, { payload }) => {
        state.isLoading = false;
        const itemIndex = state.cartItems.findIndex(
          (item) => item.productId === payload.productId
        );
        if (itemIndex !== -1) {
          state.cartItems.splice(itemIndex, 1, {
            productId: payload.productId,
            quantity: payload.quantity,
          });
        }
        setLocalCart(state.cartItems);
      })
      .addCase(updateItem.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload ?? { message: "Error. " };
      })

      // deleteItem
      .addCase(deleteItem.pending, (state, action) => {
        state.isLoading = true;
        state.error = undefined;
      })
      .addCase(deleteItem.fulfilled, (state, { payload }) => {
        const itemIndex = state.cartItems.findIndex(
          (item) => item.productId === payload.productId
        );
        if (itemIndex !== -1) {
          state.cartItems.splice(itemIndex, 1);
        }
        state.isLoading = false;
        setLocalCart(state.cartItems);
      })
      .addCase(deleteItem.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload ?? { message: "Error. " };
      });
  },
});

export const { clearCart } = cartSlice.actions

export default cartSlice.reducer;
