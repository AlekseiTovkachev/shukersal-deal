import { Member } from '../types/appTypes';
import { apiClient } from './apiClient';
import { apiErrorHandlerWrapper } from './util';

export interface LoginPostData {
    username: string;
    password: string;
};

export type TokenResponseBody = {
    member: Member,
    token: string
};


export const authApi = {
    login: (credentials: LoginPostData): Promise<TokenResponseBody> =>
        apiErrorHandlerWrapper(apiClient.post('auth/login/', credentials)),
    register: () =>
        console.error('NOT IMPLEMENTED!') //TODO: IMPLEMENT apiErrorHandlerWrapper(noAuthApiClient.post(''))
} 