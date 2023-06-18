import { ApiResponse, ApiResponseListData } from "../types/apiTypes";
import { Notification } from "../types/appTypes";
import { apiClient } from "./apiClient";
import { apiErrorHandlerWrapper } from "./util";

const notificationsServiceName = "notificationservice";

export const notificationsApi = {
  getAll: (memberId: number): Promise<ApiResponseListData<Notification>> =>
    apiErrorHandlerWrapper(
      apiClient.get(
        `${notificationsServiceName}/notifications/member/${memberId}/`
      )
    ),
  delete: (notificationId: number): Promise<ApiResponse<any>> =>
    apiErrorHandlerWrapper(
      apiClient.delete(
        `${notificationsServiceName}/noifications/${notificationId}/`
      ) // noifications <-- good one
    ),
  deleteAll: (memberId: number): Promise<ApiResponse<any>> =>
    apiErrorHandlerWrapper(
      apiClient.delete(
        `${notificationsServiceName}/notifications/member/${memberId}/`
      )
    ),
};
