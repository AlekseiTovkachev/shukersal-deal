import { ApiListData, ApiResponse, ApiResponseListData } from '../types/apiTypes';
import { Member, Product, Store } from '../types/appTypes';
import { apiClient } from './apiClient';
import { apiErrorHandlerWrapper } from './util';


export interface StorePostData {
    name: string,
    description: string
};

export interface StorePatchData {
    name?: string,
    description?: string
};

const serviceName = 'storeservice';

export const storesApi = {
    getAll: (): Promise<ApiResponseListData<Store>> =>
        apiErrorHandlerWrapper(apiClient.post(`${serviceName}/`)),

    get: (storeId: number): Promise<ApiResponse<Store>> =>
        apiErrorHandlerWrapper(apiClient.get(`${serviceName}/${storeId}/`)),

    create: (postData: StorePostData): Promise<ApiResponse<Store>> =>
        apiErrorHandlerWrapper(apiClient.post(`${serviceName}/`, postData)),

    patch: (storeId: number, newData: Partial<Store>): Promise<ApiResponse<Store>> =>
        apiErrorHandlerWrapper(apiClient.patch(`${serviceName}/${storeId}/`, newData)),

    delete: (storeId: number): Promise<ApiResponse<undefined>> =>
        apiErrorHandlerWrapper(apiClient.delete(`${serviceName}/${storeId}/`)),
} 

export const storeProductsApi = {
    getAll: (storeId: number): Promise<ApiResponseListData<Product>> =>
        apiErrorHandlerWrapper(apiClient.post(`${serviceName}/stores/${storeId}/products/`)),

    get: (storeId: number, productId: number): Promise<ApiResponse<Product>> =>
        apiErrorHandlerWrapper(apiClient.get(`${serviceName}/stores/${storeId}/products/${productId}/`)),

    create: (storeId: number, postData: StorePostData): Promise<ApiResponse<Product>> =>
        apiErrorHandlerWrapper(apiClient.post(`${serviceName}/stores/${storeId}/products/`, postData)),

    patch: (storeId: number, productId: number, newData: Partial<Store>): Promise<ApiResponse<Product>> =>
        apiErrorHandlerWrapper(apiClient.patch(`${serviceName}/stores/${storeId}/products/${productId}/`, newData)),

    delete: (storeId: number, productId: number): Promise<ApiResponse<undefined>> =>
        apiErrorHandlerWrapper(apiClient.delete(`${serviceName}/stores/${storeId}/products/${productId}/`)),
} 