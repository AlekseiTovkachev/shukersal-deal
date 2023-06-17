import { localStorageValues } from "../_configuration";
import { CartItem } from "../types/appTypes";

export const setLocalCart = (items: CartItem[]): void => {
  window.localStorage.setItem(
    localStorageValues.cartItems.name,
    JSON.stringify(items)
  );
};
