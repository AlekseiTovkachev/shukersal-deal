import { ApiResponse, ApiResponseListData } from "../types/apiTypes";
import { Product, Store } from "../types/appTypes";
import { apiClient } from "./apiClient";
import { apiErrorHandlerWrapper } from './util';

export interface StorePostData {
  name: string;
  description: string;
}

export interface StorePatchData {
  name?: string;
  description?: string;
}

const storeServiceName = "stores";
const sellerServiceName = "storemanagers";

export const storesApi = {
  getAll: (): Promise<ApiResponseListData<Store>> =>
    apiErrorHandlerWrapper(apiClient.get(`${storeServiceName}/`)),

  getMyStores: (memberId: number): Promise<ApiResponseListData<Store>> =>
    apiErrorHandlerWrapper(apiClient.get(`${sellerServiceName}/member/${memberId}/stores/`)),

  get: (storeId: number): Promise<ApiResponse<Store>> =>
    apiErrorHandlerWrapper(apiClient.get(`${storeServiceName}/${storeId}/`)),

  create: (postData: StorePostData): Promise<ApiResponse<Store>> =>
    apiErrorHandlerWrapper(apiClient.post(`${storeServiceName}/`, postData)),

  patch: (
    storeId: number,
    newData: Partial<Store>
  ): Promise<ApiResponse<Store>> =>
    apiErrorHandlerWrapper(
      apiClient.patch(`${storeServiceName}/${storeId}/`, newData)
    ),

  delete: (storeId: number): Promise<ApiResponse<undefined>> =>
    apiErrorHandlerWrapper(apiClient.delete(`${storeServiceName}/${storeId}/`)),
};

export const storeProductsApi = {
  getAll: (storeId: number): Promise<ApiResponseListData<Product>> =>
    apiErrorHandlerWrapper(
      apiClient.get(`${storeServiceName}/${storeId}/products/`)
    ),

  get: (storeId: number, productId: number): Promise<ApiResponse<Product>> =>
    apiErrorHandlerWrapper(
      apiClient.get(`${storeServiceName}/${storeId}/products/${productId}/`)
    ),

  create: (
    storeId: number,
    postData: StorePostData
  ): Promise<ApiResponse<Product>> =>
    apiErrorHandlerWrapper(
      apiClient.post(`${storeServiceName}/${storeId}/products/`, postData)
    ),

  patch: (
    storeId: number,
    productId: number,
    newData: Partial<StorePatchData>
  ): Promise<ApiResponse<Product>> =>
    apiErrorHandlerWrapper(
      apiClient.patch(
        `${storeServiceName}/${storeId}/products/${productId}/`,
        newData
      )
    ),

  delete: (
    storeId: number,
    productId: number
  ): Promise<ApiResponse<undefined>> =>
    apiErrorHandlerWrapper(
      apiClient.delete(
        `${storeServiceName}/${storeId}/products/${productId}/`
      )
    ),
};
