import { Member } from '../types/appTypes';
import { apiClient } from './apiClient';
import { apiErrorHandlerWrapper } from './util';


export interface LoginPostData {
    username: string;
    password: string;
};

export interface RegisterPostData {
    username: string;
    password: string;
};

export type TokenResponseBody = {
    member: Member,
    token: string
};

const serviceName = 'authservice';

export const authApi = {
    login: (credentials: LoginPostData): Promise<TokenResponseBody> =>
        apiErrorHandlerWrapper(apiClient.post(`${serviceName}/login/`, credentials)),
    register: (credentials: RegisterPostData) =>
        apiErrorHandlerWrapper(apiClient.post(`${serviceName}/register/`, credentials)),
} 