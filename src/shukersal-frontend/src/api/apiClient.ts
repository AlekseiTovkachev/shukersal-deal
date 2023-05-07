import axios, { InternalAxiosRequestConfig } from 'axios';
import { store } from '../redux/store';
import { API_URL } from '../_configuration';

export const apiClient = axios.create({
    baseURL: API_URL
});

apiClient.interceptors.request.use((config) => {

    // Inject authorization header
    const currentMemberData = store.getState().auth.data;
    if (currentMemberData) {
            config.headers["Authorization"] = 'Token ' + currentMemberData.token;
    }

    return config;
});