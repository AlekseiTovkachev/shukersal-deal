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

    getSlectedDiscount: (storeId: number): Promise<ApiResponseListData<DiscountRule>> =>
        apiErrorHandlerWrapper(apiClient.get(`/store/${storeId}/discounts/applied`)),

    getAllPR: (storeId: number): Promise<ApiResponseListData<PurchaseRule>> =>
        apiErrorHandlerWrapper(apiClient.get(`/stores/${storeId}/purchaserules/all`)),
    getSelectedPR: (storeId: number): Promise<ApiResponseListData<PurchaseRule>> =>
        apiErrorHandlerWrapper(apiClient.get(`/stores/${storeId}/purchaserules/applied`)),

  createNewDiscount: (storeId: number, discountType : number, discount: number, discountOnString: string): Promise<ApiResponse<boolean>> =>
      apiErrorHandlerWrapper(apiClient.post(`${discountsServiceName}`, {
          discountType: discountType % 10,
          discount: discount,
          discountOn: Math.floor(discountType / 10),
          discountOnString: discountOnString,
          storeId: storeId
      })),

    createChildDiscount: (storeId: number, discountType: number, discount: number, discountOnString: string, parent: number): Promise<ApiResponse<boolean>> =>
        apiErrorHandlerWrapper(apiClient.post(`${discountsServiceName}/${parent}`, {
            discountType: discountType % 10,
            discount: discount,
            discountOn: Math.floor(discountType / 10),
            discountOnString: discountOnString,
            storeId: storeId
        })),

    selectDiscount: (storeId: number, discountId: number): Promise<ApiResponse<boolean>> =>
        apiErrorHandlerWrapper(apiClient.request({
            method: 'PATCH',
            url: `/stores/${storeId}/${discountsServiceName}/${discountId}`,
        })),

    selectPRule: (storeId: number, discountId: number): Promise<ApiResponse<boolean>> =>
        apiErrorHandlerWrapper(apiClient.request({
            method: 'PATCH',
            url: `/stores/${storeId}/purchaserules/${discountId}`,
        })),

    createConditionalDiscount: (storeId: number, discountType: number, discountlimit: number, discountOnString: string, minhour: number, maxhour: number, parent: number): Promise<ApiResponse<boolean>> =>
        apiErrorHandlerWrapper(apiClient.post(`${discountsServiceName}/boolean/new/${parent}`, {
            discountRuleBooleanType: discountType,
            conditionString: discountOnString,
            conditionLimit: discountlimit,
            minHour: minhour,
            maxHour: maxhour,
            storeId: storeId
        })),

    createConditionalChildDiscount: (storeId: number, discountType: number, discountlimit: number, discountOnString: string, minhour: number, maxhour: number, parent: number): Promise<ApiResponse<boolean>> =>
        apiErrorHandlerWrapper(apiClient.post(`${discountsServiceName}/boolean/child/${parent}`, {
            discountRuleBooleanType: discountType,
            conditionString: discountOnString,
            conditionLimit: discountlimit,
            minHour: minhour,
            maxHour: maxhour,
            storeId: storeId
        })),

    createPR: (storeId: number, discountType: number, discountlimit: number, discountOnString: string, minhour: number, maxhour: number): Promise<ApiResponse<boolean>> =>
        apiErrorHandlerWrapper(apiClient.post(`/purchaserules`, {
            purchaseRuleType: discountType,
            conditionString: discountOnString,
            conditionLimit: discountlimit,
            minHour: minhour,
            maxHour: maxhour,
            storeId: storeId
        })),

    createChildPR: (storeId: number, discountType: number, discountlimit: number, discountOnString: string, minhour: number, maxhour: number, parent: number): Promise<ApiResponse<boolean>> =>
        apiErrorHandlerWrapper(apiClient.post(`/purchaserules/${parent}`, {
            purchaseRuleType: discountType,
            conditionString: discountOnString,
            conditionLimit: discountlimit,
            minHour: minhour,
            maxHour: maxhour,
            storeId: storeId
        })),
};