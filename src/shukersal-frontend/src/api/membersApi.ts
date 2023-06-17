import { ApiResponse, ApiResponseListData } from "../types/apiTypes";
import { Member } from "../types/appTypes";
import { apiClient } from "./apiClient";
import { apiErrorHandlerWrapper } from "./util";

const serviceName = "members";

export const membersApi = {
  getAll: (): Promise<ApiResponseListData<Member>> =>
    apiErrorHandlerWrapper(apiClient.get(`${serviceName}/`)),
};
