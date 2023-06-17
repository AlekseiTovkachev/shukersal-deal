import { ApiResponse } from "../types/apiTypes";
import { CartItem, ShoppingCart, ShoppingItem } from "../types/appTypes";
import { apiClient } from "./apiClient";
import { apiErrorHandlerWrapper } from "./util";

const cartServiceName = "shoppingcarts";

export const cartApi = {
  getCart: (memberId: number): Promise<ApiResponse<ShoppingCart>> =>
    apiErrorHandlerWrapper(apiClient.get(`${cartServiceName}/member/${memberId}/`)),

  addItem: (
    cartId: number,
    postData: CartItem
  ): Promise<ApiResponse<ShoppingItem>> =>
    apiErrorHandlerWrapper(
      apiClient.post(`${cartServiceName}/${cartId}/items/`, postData)
    ),

  updateItem: (
    cartId: number,
    putData: CartItem
  ): Promise<ApiResponse<ShoppingItem>> =>
    apiErrorHandlerWrapper(
      apiClient.put(`${cartServiceName}/${cartId}/items/`, putData)
    ),

  deleteItem: (
    cartId: number,
    productId: number
  ): Promise<ApiResponse<ShoppingItem>> =>
    apiErrorHandlerWrapper(
      apiClient.delete(
        `${cartServiceName}/${cartId}/items?productId=${productId}`
      )
    ),
};
