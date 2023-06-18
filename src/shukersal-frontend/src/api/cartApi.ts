import { ApiResponse } from "../types/apiTypes";
import {
  CartItem,
  ShoppingCart,
  ShoppingItem,
  Transaction,
} from "../types/appTypes";
import { apiClient } from "./apiClient";
import { apiErrorHandlerWrapper } from "./util";

export interface TransactionPostData {
  isMember: boolean;
  memberId: number | undefined;
  transactionDate: string;
  billingDetails: {
    holderFirstName: string;
    holderLastName: string;
    holderID: string; // length: 9
    cardNumber: string; // length: 16
    expirationDate: string; // format: "2023-06-17"
    cvc: string; // length: 3
  };
  deliveryDetails: {
    receiverFirstName: string;
    receiverLastName: string;
    receiverAddress: string;
    receiverCity: string;
    receiverCountry: string;
    receiverPostalCode: string; // length: 7
  };
  transactionItems: CartItem[];
  totalPrice: number;
}

const cartServiceName = "shoppingcarts";
const transactionServiceName = "transactions";

export const cartApi = {
  getCart: (memberId: number): Promise<ApiResponse<ShoppingCart>> =>
    apiErrorHandlerWrapper(
      apiClient.get(`${cartServiceName}/member/${memberId}/`)
    ),

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

  submitTransaction: (
    postData: TransactionPostData
  ): Promise<ApiResponse<Transaction>> =>
    apiErrorHandlerWrapper(
      apiClient.post(`${transactionServiceName}/`, postData)
    ),
};
