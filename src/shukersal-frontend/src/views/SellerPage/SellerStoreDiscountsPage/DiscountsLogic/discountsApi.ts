import { apiClient } from "../../../../api/apiClient";
import { apiErrorHandlerWrapper } from "../../../../api/util";
import { ApiResponse, ApiResponseListData } from "../../../../types/apiTypes";

const discountsServiceName = "discounts";

export interface DiscountPostData {
  name: string;
  //...
}

export const discountsApi = {
  getAll: (storeId: number): Promise<ApiResponseListData<DiscountRule>> =>
    apiErrorHandlerWrapper(apiClient.get(`${discountsServiceName}/all/${storeId}`)),

  create: (storeId: number, postData: DiscountPostData): Promise<ApiResponse<DiscountRule>> =>
    apiErrorHandlerWrapper(apiClient.post(`${discountsServiceName}/${storeId}`, postData)), // Not sure about this endpoint
};