import axios, { InternalAxiosRequestConfig } from "axios";
import { API_URL } from "../_configuration";
import { store } from "../redux/store";

export const apiClient = axios.create({
  baseURL: API_URL,
});

apiClient.interceptors.request.use((config) => {
  // Inject authorization header
  const currentMemberData = store.getState().auth.data;
  if (currentMemberData) {
    config.headers["Authorization"] = "Bearer " + currentMemberData.token;
  }
  return config;
});
