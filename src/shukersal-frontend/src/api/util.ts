import { AxiosResponse } from "axios";
import { ApiResponse, ApiResponseListData } from "../types/apiTypes";

export const apiErrorHandlerWrapper = <T>(
  promise: Promise<AxiosResponse>
): Promise<T> => {
  return promise
    .then((res) => {
        if (res.status >= 400 && res.status < 500)
        Promise.reject({
          message: res.data,
        });
      if (res.status >= 500 && res.status < 600)
        Promise.reject({
          message: res.data,
        });
      return Promise.resolve(res.data);
    })
    .catch((err) =>
      Promise.reject({
        message: err.response?.data ?? err.message ?? "Error",
      })
    );
};
