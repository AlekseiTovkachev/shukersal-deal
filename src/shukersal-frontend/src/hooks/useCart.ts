import { useCallback, useEffect } from "react";
import { useAuth } from "./useAuth";
import { useAppDispatch } from "./useAppDispatch";
import { CartItem } from "../types/appTypes";
import { addItem, deleteItem, getCart, updateItem } from "../redux/cartSlice";
import { useAppSelector } from "./useAppSelector";

export const useCart = () => {
  const dispatch = useAppDispatch();

  const isLoading = useAppSelector((state) => state.cart.isLoading);
  const error = useAppSelector((state) => state.cart.error);
  const cartId = useAppSelector((state) => state.cart.cartId);
  const cartItems = useAppSelector((state) => state.cart.cartItems);
  const authData = useAuth();
  const memberId = authData.currentMemberData?.currentMember.id;

  const getCartCallback = useCallback(async () => {
    if (memberId) {
      const response = await dispatch(getCart(memberId));
      if (response.meta.requestStatus === "fulfilled") {
        return true;
      }
      return false;
    }
  }, [dispatch, memberId]);

  const addItemCallback = useCallback(
    async (item: CartItem) => {
      const response = await dispatch(
        addItem({
          cartId: cartId,
          item: item,
        })
      );
      if (response.meta.requestStatus === "fulfilled") {
        return true;
      }
      return false;
    },
    [dispatch, cartId]
  );

  const updateItemCallback = useCallback(
    async (item: CartItem) => {
      const response = await dispatch(
        updateItem({
          cartId: cartId,
          item: item,
        })
      );
      if (response.meta.requestStatus === "fulfilled") {
        return true;
      }
      return false;
    },
    [dispatch, cartId]
  );

  const deleteItemCallback = useCallback(
    async (item: CartItem) => {
      const response = await dispatch(
        deleteItem({
          cartId: cartId,
          productId: item.productId,
        })
      );
      if (response.meta.requestStatus === "fulfilled") {
        return true;
      }
      return false;
    },
    [dispatch, cartId]
  );

  return {
    isLoading: isLoading,
    error: error,
    cartItems: cartItems,
    getCart: getCartCallback,
    addItem: addItemCallback,
    updateItem: updateItemCallback,
    deleteItem: deleteItemCallback,
  };
};
